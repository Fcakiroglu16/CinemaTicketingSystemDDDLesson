using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;

namespace CinemaTicketingSystem.Persistence.Ticketing;

internal class PurchaseRepository(AppDbContext context)
    : GenericRepository<Purchase>(context), IPurchaseRepository
{
    public List<Purchase> GetTicketsPurchaseByScheduleId(Guid scheduleId)
    {
        return _context.Purchases.Where(x => x.ScheduledMovieShowId == scheduleId).ToList();
    }
}