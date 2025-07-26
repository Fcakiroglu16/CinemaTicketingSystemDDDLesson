using CinemaTicketingSystem.Domain.Scheduling;
using CinemaTicketingSystem.Domain.Scheduling.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketingSystem.Persistence.Scheduling;

internal class ScheduleRepository(AppDbContext context)
    : GenericRepository<Guid, Schedule>(context), IScheduleRepository
{
    public Task<List<Schedule>> GetMoviesByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default)
    {
        return context.Schedules
            .Where(x => x.HallId == hallId).OrderBy(x => x.ShowTime.StartTime)
            .ToListAsync(cancellationToken);
    }
}