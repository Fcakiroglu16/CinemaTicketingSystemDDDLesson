#region

using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Holds;
using Microsoft.EntityFrameworkCore;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Ticketing;

public class SeatHoldRepository(AppDbContext context) : GenericRepository<SeatHold>(context), ISeatHoldRepository
{
    public Task<List<SeatHold>> GetConfirmedListByScheduleIdAndScreeningDate(Guid scheduledMovieShowId,
        DateOnly ScreeningDate)
    {
        return _context.SeatHolds
            .Where(x => x.ScheduledMovieShowId == scheduledMovieShowId && x.ScreeningDate == ScreeningDate &&
                        x.Status == HoldStatus.Hold)
            .ToListAsync();
    }
}