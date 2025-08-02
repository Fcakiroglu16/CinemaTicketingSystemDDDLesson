using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;

public interface IScheduleRepository : IGenericRepository<Guid, Schedule>
{
    Task<List<Schedule>> GetMoviesByHallIdAsync(Guid hallId, CancellationToken cancellationToken = default);
}