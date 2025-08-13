#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;
using CinemaTicketingSystem.Persistence;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

internal class PurchaseRepository(AppDbContext context)
    : GenericRepository<Purchase>(context), IPurchaseRepository
{
    public List<Purchase> GetTicketsPurchaseByScheduleIdAndScreeningDate(Guid scheduleId, DateOnly ScreeningDate)
    {
        return _context.Purchases.Where(x => x.ScheduledMovieShowId == scheduleId && x.ScreeningDate == ScreeningDate)
            .ToList();
    }
}