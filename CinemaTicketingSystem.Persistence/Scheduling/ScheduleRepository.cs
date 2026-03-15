#region

using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Persistence.Scheduling;

internal class ScheduleRepository(AppDbContext context)
    : GenericRepository<Schedule>(context), IScheduleRepository;