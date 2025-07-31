using CinemaTicketingSystem.Domain.Ticketing;
using CinemaTicketingSystem.Domain.Ticketing.Repositories;

namespace CinemaTicketingSystem.Persistence.Ticketing;

internal class TicketPurchaseRepository(AppDbContext context)
    : GenericRepository<Guid, TicketPurchase>(context), ITicketPurchaseRepository
{
    public List<TicketPurchase> GetTicketsPurchaseByScheduleId(Guid scheduleId)
    {
        return _context.TicketPurchases.Where(x => x.ScheduleId == scheduleId).ToList();
    }
}