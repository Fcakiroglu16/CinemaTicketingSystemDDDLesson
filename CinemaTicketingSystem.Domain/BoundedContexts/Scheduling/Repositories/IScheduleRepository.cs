#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;

public interface IScheduleRepository : IGenericRepository<Schedule>;