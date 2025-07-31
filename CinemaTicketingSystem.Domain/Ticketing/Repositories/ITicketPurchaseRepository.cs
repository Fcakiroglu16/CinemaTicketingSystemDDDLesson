using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.Ticketing.Repositories;

public interface ITicketPurchaseRepository : IGenericRepository<Guid, TicketPurchase>
{
    List<TicketPurchase> GetTicketsPurchaseByScheduleId(Guid scheduleId);
}