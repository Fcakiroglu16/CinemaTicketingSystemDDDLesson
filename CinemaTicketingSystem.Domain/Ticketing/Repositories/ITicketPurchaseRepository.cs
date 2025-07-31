using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.Domain.Ticketing.Tickets;

namespace CinemaTicketingSystem.Domain.Ticketing.Repositories;

public interface ITicketPurchaseRepository : IGenericRepository<Guid, TicketPurchase>
{
    List<TicketPurchase> GetTicketsPurchaseByScheduleId(Guid scheduleId);
}