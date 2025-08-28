#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

public class SeatHoldRepository(AppDbContext context) : GenericRepository<SeatHold>(context), ISeatHoldRepository
{

    private Task Confirm()



}