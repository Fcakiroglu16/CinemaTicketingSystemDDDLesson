#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Schedules.ICL;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
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
    ReservationEligibilityPolicy reservationEligibilityPolicy) : IScopedDependency, IReservationAppService
{
    public async Task<AppResult> Create(ReserveSeatsRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var isReservationTooLate =
            reservationEligibilityPolicy.IsReservationTooLate(scheduleInfo.Data!.showTime.StartTime,
                request.ScreeningDate);


        if (!isReservationTooLate.IsSuccess)
            return appDependencyService.LocalizeError.Error(isReservationTooLate.Error!,
                isReservationTooLate.ErrorData);


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return catalogInfo;


        var reservationList =
            (await reservationRepository.WhereAsync(x =>
                x.ScheduledMovieShowId == request.ScheduledMovieShowId && x.ScreeningDate == request.ScreeningDate))
            .ToList();


        var reservationCount = reservationList.Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - reservationCount;

        if (availableSeatCount <= 0)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatNotAvailable);


        if (request.SeatPositionList.Count > availableSeatCount)
            return appDependencyService.LocalizeError.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount]);


        var seatHoldList = (await seatHoldRepository.WhereAsync(x =>
            x.ScheduledMovieShowId == request.ScheduledMovieShowId &&
            x.CustomerId == appDependencyService.UserContext.UserId)).ToList();


        var IsValidateOwnershipAndValidityResult = reservationEligibilityPolicy.ValidateOwnershipAndValidity(
            seatHoldList,
            request.SeatPositionList.Select(x => new SeatPosition(x.Row, x.Number)).ToList());


        if (!IsValidateOwnershipAndValidityResult.IsSuccess)
            return appDependencyService.LocalizeError.Error(IsValidateOwnershipAndValidityResult.Error!,
                IsValidateOwnershipAndValidityResult.ErrorData);


        foreach (var seatPosition in from seatPosition in request.SeatPositionList
                 let seatNumber = new SeatPosition(seatPosition.Row, seatPosition.Number)
                 let hasSeat = reservationList.Any(r => r.HasSeat(seatNumber))
                 where hasSeat
                 select seatPosition)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatAlreadyReserved,
                [seatPosition.Row, seatPosition.Number]);


        var reservation = new Reservation(request.ScheduledMovieShowId, appDependencyService.UserContext.UserId,
            request.ScreeningDate, scheduleInfo.Data.showTime.StartTime);


        foreach (var seatPosition in request.SeatPositionList.Select(seatPositionDto =>
                     new SeatPosition(seatPositionDto.Row, seatPositionDto.Number)))
            reservation.AddSeat(new ReservationSeat(seatPosition));

        reservation.Confirm();


        await reservationRepository.AddAsync(reservation);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}