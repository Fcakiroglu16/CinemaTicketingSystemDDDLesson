#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Purchases;

public interface IPurchaseRepository : IGenericRepository<Purchase>
{
    List<Purchase> GetTicketsPurchaseByScheduleIdAndScreeningDate(Guid scheduleId, DateOnly ScreeningDate);
}