#region

using CinemaTicketingSystem.Application.Catalog.Services;
using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Schedules.Services;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds.Specifications;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;


#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class ReservationAppService(
    AppDependencyService appDependencyService,
    IScheduleQueryService iScheduleQueryService,
    ICatalogQueryService catalogQueryService,
    IReservationRepository reservationRepository,
    ISeatHoldRepository seatHoldRepository,
    ITicketIssuanceRepository ticketIssuanceRepository,
    ReservationEligibilityPolicy reservationEligibilityPolicy) : IScopedDependency, IReservationAppService
{
    public async Task<AppResult<CreateReservationResponse>> Create(CreateReservationRequest request)
    {
        AppResult<GetScheduleInfoResponse> scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return AppResult<CreateReservationResponse>.Error(scheduleInfo.ProblemDetails!);


        DomainResult isReservationTooLate =
            reservationEligibilityPolicy.IsReservationTooLate(scheduleInfo.Data!.showTime.StartTime,
                request.ScreeningDate);


        if (!isReservationTooLate.IsSuccess)
            return appDependencyService.LocalizeError.Error<CreateReservationResponse>(isReservationTooLate.Error!,
                isReservationTooLate.ErrorData);


        Guid userId = appDependencyService.UserContext.UserId;
        List<SeatHold> seatHoldList = await seatHoldRepository.ListAsync(
            new UserSeatHoldsByScheduleAndDateSpec(userId, request.ScheduledMovieShowId, request.ScreeningDate));


        if (seatHoldList.Any(seatHold => seatHold.IsExpired()))
            return appDependencyService.LocalizeError.Error<CreateReservationResponse>(ErrorCodes.SeatHoldExpired);


        // Fetch confirmed seats from tickets
        List<SeatPosition> confirmedTicketSeatPositions =
            (await ticketIssuanceRepository.ListAsync(
                new ConfirmedTicketIssuanceByScheduleAndDateSpec(request.ScheduledMovieShowId, request.ScreeningDate)))
            .SelectMany(x => x.TicketList)
            .Select(x => x.SeatPosition)
            .ToList();

        // Fetch confirmed seats from holds
        List<SeatPosition> confirmedSeatHoldSeatPositions =
            (await seatHoldRepository.ListAsync(
                new ActiveSeatHoldsByScheduleAndDateSpec(request.ScheduledMovieShowId, request.ScreeningDate)))
            .Select(x => x.SeatPosition)
            .ToList();


        // Merge uniquely by seat coordinates
        List<SeatPosition> occupiedSeatPositions = confirmedTicketSeatPositions
            .Concat(confirmedSeatHoldSeatPositions)
            .DistinctBy(sp => (sp.Row, sp.Number))
            .ToList();


        foreach (SeatHold? seat in seatHoldList)
        {
            bool seatTaken = occupiedSeatPositions.Any(x =>
                x.Row == seat.SeatPosition.Row && x.Number == seat.SeatPosition.Number);
            if (seatTaken)
                return appDependencyService.LocalizeError.Error<CreateReservationResponse>(ErrorCodes.DuplicateSeat,
                    [seat.SeatPosition.Row, seat.SeatPosition.Number]);
        }


        Reservation reservation = new Reservation(request.ScheduledMovieShowId, appDependencyService.UserContext.UserId,
            request.ScreeningDate);


        foreach (SeatHold? seat in seatHoldList) reservation.AddSeat(new ReservationSeat(seat.SeatPosition));


        await reservationRepository.AddAsync(reservation);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult<CreateReservationResponse>.SuccessAsOk(new CreateReservationResponse(reservation.Id));
    }


    public async Task<AppResult> Confirm(Guid reservationId)
    {
        Reservation? reservation = await reservationRepository.GetByIdAsync(reservationId);


        AppResult<GetScheduleInfoResponse> scheduleInfo = await iScheduleQueryService.GetScheduleInfo(reservation.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        DomainResult isReservationTooLate =
            reservationEligibilityPolicy.IsReservationTooLate(scheduleInfo.Data!.showTime.StartTime,
                reservation.ScreeningDate);


        if (!isReservationTooLate.IsSuccess)
            return appDependencyService.LocalizeError.Error(isReservationTooLate.Error!,
                isReservationTooLate.ErrorData);


        AppResult<GetCatalogInfoResponse> catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return catalogInfo;


        List<Reservation> reservationList =
            (await reservationRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == reservation.ScheduledMovieShowId &&
                x.ScreeningDate == reservation.ScreeningDate && x.Status == ReservationStatus.Confirmed))
            .ToList();


        int reservationCount = reservationList.Count;

        int availableSeatCount = catalogInfo.Data!.SeatCount - reservationCount;

        if (availableSeatCount <= 0)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatNotAvailable);


        if (reservation.ReservationSeatList.Count > availableSeatCount)
            return appDependencyService.LocalizeError.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount]);


        List<ReservationSeat> hasReservationSeats = (from reservationSeat in reservation.ReservationSeatList
                                                     let hasSeat = reservationList.Any(r => r.HasSeat(reservationSeat.SeatPosition))
                                                     where hasSeat
                                                     select reservationSeat).ToList();


        if (hasReservationSeats.Any())
        {
            ReservationSeat reservationSeat = hasReservationSeats.First();

            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyReserved,
                [reservationSeat.SeatPosition.Row, reservationSeat.SeatPosition.Number]);
        }


        List<SeatHold> seatHoldList = (await seatHoldRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == reservation.ScheduledMovieShowId &&
                x.CustomerId == appDependencyService.UserContext.UserId &&
                x.ScreeningDate == reservation.ScreeningDate))
            .ToList();


        if (seatHoldList.Any(seatHold => seatHold.IsExpired()))
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatHoldExpired);

        DomainResult IsValidateOwnershipAndValidityResult = reservationEligibilityPolicy.ValidateOwnershipAndValidity(
            seatHoldList,
            reservation.ReservationSeatList.Select(x => x.SeatPosition).ToList());


        if (!IsValidateOwnershipAndValidityResult.IsSuccess)
            return appDependencyService.LocalizeError.Error(IsValidateOwnershipAndValidityResult.Error!,
                IsValidateOwnershipAndValidityResult.ErrorData);


        reservation.Confirm(scheduleInfo.Data.showTime.StartTime);
        await reservationRepository.UpdateAsync(reservation);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    Task<AppResult<CreateReservationResponse>> IReservationAppService.Create(CreateReservationRequest request)
    {
        throw new NotImplementedException();
    }

    Task<AppResult> IReservationAppService.Confirm(Guid reservationId)
    {
        throw new NotImplementedException();
    }
}