#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Schedules.ICL;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class PurchaseAppService(
    AppDependencyService appDependencyService,
    IPurchaseRepository purchaseRepository,
    IUserContext userContext,
    ICatalogQueryService catalogQueryService,
    IScheduleQueryService iScheduleQueryService,
    ISeatHoldRepository seatHoldRepository,
    IReservationRepository reservationRepository) : IScopedDependency, ITicketPurchaseAppService
{
    public async Task<AppResult> Create(PurchaseTicketRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);


        var ticketPurchaseList =
            purchaseRepository.GetTicketsPurchaseByScheduleIdAndScreeningDate(request.ScheduledMovieShowId,
                request.ScreeningDate);


        var purchasedTicketCount = ticketPurchaseList.SelectMany(x => x.TicketList).Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - purchasedTicketCount;
        if (availableSeatCount <= 0)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatNotAvailable);


        if (availableSeatCount < request.SeatPositionList.Count)
            return appDependencyService.LocalizeError.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount]);


        foreach (var seat in request.SeatPositionList)
        {
            var seatNumber = new SeatPosition(seat.Row, seat.Number);
            var hasTicket = ticketPurchaseList.Any(x => x.HasTicketForSeat(seatNumber));
            if (hasTicket)
                return appDependencyService.LocalizeError.Error(ErrorCodes.DuplicateSeat, [seat.Row, seat.Number]);
        }


        var seatHoldList = (await seatHoldRepository.WhereAsync(x =>
            x.ScheduledMovieShowId == request.ScheduledMovieShowId &&
            x.CustomerId == userContext.UserId)).ToList();


        if (seatHoldList.Count() != request.SeatPositionList.Count)
        {
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatHoldNotFound);
        }

        foreach (var seatHold in seatHoldList)
        {
            var exist = request.SeatPositionList.Any(seat =>
                seatHold.SeatPosition == new SeatPosition(seat.Row, seat.Number));


            if (!exist)
            {
                return appDependencyService.LocalizeError.Error(ErrorCodes.SeatHoldNotFound,
                    [seatHold.SeatPosition.Row, seatHold.SeatPosition.Number]);
            }


            if (!seatHold.CanBeConvertedToReservationOrPurchase())
            {
                return appDependencyService.LocalizeError.Error(ErrorCodes.SeatHoldExpired,
                    [seatHold.SeatPosition.Row, seatHold.SeatPosition.Number]);
            }
        }


        var purchase = new Purchase(request.ScheduledMovieShowId, userContext.UserId, request.ScreeningDate);

        foreach (var seat in request.SeatPositionList)
        {
            var newTicket = new Ticket(new SeatPosition(seat.Row, seat.Number), scheduleInfo.Data.TicketPrice);
            purchase.AddTicket(newTicket);
        }


        await purchaseRepository.AddAsync(purchase);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }


    public async Task<AppResult> CreateFromReservation(Guid ReservationId)
    {
        var reservation = await reservationRepository.GetByIdAsync(ReservationId);


        if (reservation!.IsExpired())
        {
            return appDependencyService.LocalizeError.Error(ErrorCodes.ReservationExpired);
        }

        var purchase = new Purchase(reservation.ScheduledMovieShowId, reservation.CustomerId,
            reservation.ScreeningDate);


        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(reservation.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;

        foreach (var seat in reservation.ReservationSeatList)
        {
            var newTicket = new Ticket(seat.SeatPosition, scheduleInfo.Data!.TicketPrice);
            purchase.AddTicket(newTicket);
        }


        await purchaseRepository.AddAsync(purchase);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}