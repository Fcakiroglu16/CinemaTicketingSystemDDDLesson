using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Application.Catalog.ICL;
using CinemaTicketingSystem.Application.Schedules.ICL;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing;
using CinemaTicketingSystem.Domain.Ticketing.Repositories;
using CinemaTicketingSystem.Domain.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Ticketing;

public class TicketPurchaseAppService(
    AppDependencyService appDependencyService,
    ITicketPurchaseRepository ticketPurchaseRepository,
    IUserContext userContext,
    ICatalogQueryService catalogQueryService,
    IScheduleQueryService iScheduleQueryService) : IScopedDependency, ITicketPurchaseAppService
{
    public async Task<AppResult> Purchase(PurchaseTicketRequest request)
    {
        var scheduleInfo = await iScheduleQueryService.GetScheduleInfo(request.ScheduleId);

        if (scheduleInfo.IsFail) return scheduleInfo;


        var catalogInfo =
            await catalogQueryService.GetCinemaInfo(scheduleInfo.Data!.CinemaHallId, scheduleInfo.Data.MovieId);


        var ticketPurchaseList = ticketPurchaseRepository.GetTicketsPurchaseByScheduleId(request.ScheduleId);


        var purchasedTicketCount = ticketPurchaseList.SelectMany(x => x.TicketList).Count();


        // hallId sahip hall toplam kaç kişi alıyor
        // seçilen koltuk ilgili hall'de bulunuyor mu?
        // seçilen koltuk daha önce satın alınmış mı?
        //cinema name,hall name,show time bilgileri lazım (scheduleId üzerinden alınabilir)


        var availableSeatCount = catalogInfo.Data!.SeatCount - purchasedTicketCount;
        if (availableSeatCount <= 0)
            return appDependencyService.Error(ErrorCodes.SeatNotAvailable,
                HttpStatusCode.BadRequest);


        if (availableSeatCount < request.seats.Count)
            return appDependencyService.Error(ErrorCodes.NotEnoughSeatsAvailable, [availableSeatCount],
                HttpStatusCode.BadRequest);


        foreach (var seat in request.seats)
        {
            var seatNumber = new SeatNumber(seat.Row, seat.Number);
            var hasTicket = ticketPurchaseList.Any(x => x.HasTicketForSeat(seatNumber));
            if (hasTicket)
                return appDependencyService.Error(ErrorCodes.DuplicateSeat, [seat.Row, seat.Number],
                    HttpStatusCode.BadRequest);
        }


        var ticket = new TicketPurchase(request.ScheduleId, userContext.UserId);

        foreach (var seat in request.seats)
        {
            var newTicket = new Ticket(new SeatNumber(seat.Row, seat.Number), scheduleInfo.Data.TicketPrice);
            ticket.AddTicket(newTicket);
        }


        await ticketPurchaseRepository.AddAsync(ticket);

        await appDependencyService.UnitOfWork.SaveChangesAsync();

        return AppResult.SuccessAsNoContent();
    }
}