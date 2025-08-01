using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Schedules.ICL;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Ticketing;

internal class ReservationAppService(
    AppDependencyService appDependencyService,
    IScheduleQueryService iScheduleQueryService,
    ICatalogQueryService catalogQueryService,
    IReservationRepository reservationRepository,
    IUserContext userContext) : IScopedDependency, IReservationAppService
{
    public async Task<AppResult> ReserveSeats(ReserveSeatsRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return catalogInfo;


        var reservationList =
            (await reservationRepository.WhereAsync(x => x.ScheduledMovieShowId == request.ScheduledMovieShowId))
            .ToList();


        var reservationCount = reservationList.Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - reservationCount;

        if (availableSeatCount <= 0)
            return appDependencyService.Error(ErrorCodes.SeatNotAvailable,
                HttpStatusCode.BadRequest);


        if (request.SeatPositionList.Count > availableSeatCount)
            return appDependencyService.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount],
                HttpStatusCode.BadRequest);


        foreach (var seatPosition in from seatPosition in request.SeatPositionList
                 let seatNumber = new SeatPosition(seatPosition.Row, seatPosition.Number)
                 let hasSeat = reservationList.Any(r => r.HasSeat(seatNumber))
                 where hasSeat
                 select seatPosition)
            return appDependencyService.Error(ErrorCodes.SeatAlreadyReserved,
                [seatPosition.Row, seatPosition.Number]);

        var reservation = new Reservation(request.ScheduledMovieShowId, userContext.UserId);
        foreach (var seatPosition in request.SeatPositionList.Select(seatPositionDto =>
                     new SeatPosition(seatPositionDto.Row, seatPositionDto.Number)))
            reservation.AddSeat(new ReservationSeat(seatPosition));

        reservation.Confirm();


        await reservationRepository.AddAsync(reservation);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}