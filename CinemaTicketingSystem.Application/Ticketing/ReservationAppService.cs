#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Ticketing.External;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class ReservationAppService(
    AppDependencyService appDependencyService,
    IScheduleQueryService iScheduleQueryService,
    ICatalogQueryService catalogQueryService,
    IReservationRepository reservationRepository,
    ISeatHoldRepository seatHoldRepository,
    ReservationEligibilityPolicy reservationEligibilityPolicy) : IScopedDependency, IReservationAppService
{
    public async Task<AppResult<CreateReservationResponse>> Create(CreateReservationRequest request)
    {
        var reservation = new Reservation(request.ScheduledMovieShowId, appDependencyService.UserContext.UserId,
            request.ScreeningDate);


        var seatHoldList = (await seatHoldRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == reservation.ScheduledMovieShowId &&
                x.CustomerId == appDependencyService.UserContext.UserId &&
                x.ScreeningDate == reservation.ScreeningDate))
            .ToList();


        foreach (var seatPosition in seatHoldList.Select(x => x.SeatPosition))
        {
            reservation.AddSeat(new ReservationSeat(seatPosition));
        }


        await reservationRepository.AddAsync(reservation);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult<CreateReservationResponse>.SuccessAsOk(new CreateReservationResponse(reservation.Id));
    }


    public async Task<AppResult> Confirm(Guid reservationId)
    {
        var reservation = await reservationRepository.GetByIdAsync(reservationId);


        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(reservation.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var isReservationTooLate =
            reservationEligibilityPolicy.IsReservationTooLate(scheduleInfo.Data!.showTime.StartTime,
                reservation.ScreeningDate);


        if (!isReservationTooLate.IsSuccess)
            return appDependencyService.LocalizeError.Error(isReservationTooLate.Error!,
                isReservationTooLate.ErrorData);


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return catalogInfo;


        var reservationList =
            (await reservationRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == reservation.ScheduledMovieShowId &&
                x.ScreeningDate == reservation.ScreeningDate))
            .ToList();


        var reservationCount = reservationList.Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - reservationCount;

        if (availableSeatCount <= 0)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatNotAvailable);


        if (reservation.ReservationSeatList.Count > availableSeatCount)
            return appDependencyService.LocalizeError.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount]);


        foreach (var reservationSeat in from reservationSeat in reservation.ReservationSeatList
                 let hasSeat = reservationList.Any(r => r.HasSeat(reservationSeat.SeatPosition))
                 where hasSeat
                 select reservationSeat)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyReserved,
                [reservationSeat.SeatPosition.Row, reservationSeat.SeatPosition.Number]);


        var seatHoldList = (await seatHoldRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == reservation.ScheduledMovieShowId &&
                x.CustomerId == appDependencyService.UserContext.UserId &&
                x.ScreeningDate == reservation.ScreeningDate))
            .ToList();


        var IsValidateOwnershipAndValidityResult = reservationEligibilityPolicy.ValidateOwnershipAndValidity(
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
}