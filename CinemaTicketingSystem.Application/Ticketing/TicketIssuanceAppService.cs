#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Application.Ticketing.External;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Reservations;
using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing;

public class TicketIssuanceAppService(
    AppDependencyService appDependencyService,
    ITicketIssuanceRepository ticketIssuanceRepository,
    ICatalogQueryService catalogQueryService,
    IScheduleQueryService iScheduleQueryService,
    ISeatHoldRepository seatHoldRepository,
    IReservationRepository reservationRepository) : IScopedDependency, ITicketPurchaseAppService
{
    public async Task<AppResult> Create(CreateTicketIssuanceRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);

        if (catalogInfo.IsFail) return catalogInfo;


        var userId = appDependencyService.UserContext.UserId;


        var userSeatHoldList = (await seatHoldRepository.WhereAsync(x =>
                x.CustomerId == userId &&
                x.ScheduledMovieShowId == request.ScheduledMovieShowId && x.ScreeningDate == request.ScreeningDate))
            .ToList();


        var isSeatHoldExpired = userSeatHoldList.First().IsExpired();

        if (isSeatHoldExpired) return appDependencyService.LocalizeError.Error(ErrorCodes.SeatHoldExpired);


        var confirmedTicketList =
            await ticketIssuanceRepository.GetConfirmedTicketsIssuanceByScheduleIdAndScreeningDate(
                request.ScheduledMovieShowId,
                request.ScreeningDate);


        var confirmedTicketCount = confirmedTicketList.SelectMany(x => x.TicketList).Count();

        var availableSeatCount = catalogInfo.Data!.SeatCount - confirmedTicketCount;
        if (availableSeatCount <= 0)
            return appDependencyService.LocalizeError.Error(ErrorCodes.SeatNotAvailable);


        if (availableSeatCount < userSeatHoldList.Count)
            return appDependencyService.LocalizeError.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount]);


        foreach (var seat in userSeatHoldList)
        {
            var hasTicket = confirmedTicketList.Any(x => x.HasTicketForSeat(seat.SeatPosition));
            if (hasTicket)
                return appDependencyService.LocalizeError.Error(ErrorCodes.DuplicateSeat,
                    [seat.SeatPosition.Row, seat.SeatPosition.Number]);
        }


        var newTicketIssuance =
            new TicketIssuance(request.ScheduledMovieShowId, userId, request.ScreeningDate);

        foreach (var seat in userSeatHoldList)
        {
            var newTicket = new Ticket(seat.SeatPosition, scheduleInfo.Data.TicketPrice);
            newTicketIssuance.AddTicket(newTicket);
        }


        await ticketIssuanceRepository.AddAsync(newTicketIssuance);

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

        var purchase = new TicketIssuance(reservation.ScheduledMovieShowId, reservation.CustomerId,
            reservation.ScreeningDate);


        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(reservation.ScheduledMovieShowId);

        if (scheduleInfo.IsFail) return scheduleInfo;

        foreach (var seat in reservation.ReservationSeatList)
        {
            var newTicket = new Ticket(seat.SeatPosition, scheduleInfo.Data!.TicketPrice);
            purchase.AddTicket(newTicket);
        }


        await ticketIssuanceRepository.AddAsync(purchase);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}