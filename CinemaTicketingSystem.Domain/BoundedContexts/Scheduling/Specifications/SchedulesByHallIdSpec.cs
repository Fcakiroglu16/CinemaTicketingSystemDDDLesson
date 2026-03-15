#region

using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Specifications;

/// <summary>
/// Specification for schedules filtered by hall ID, ordered by start time ascending.
/// </summary>
public sealed class SchedulesByHallIdSpec : Specification<Schedule>
{
    public SchedulesByHallIdSpec(Guid hallId)
    {
        AddCriteria(x => x.HallId == hallId);
        AddOrderBy(x => x.ShowTime.StartTime);
    }
}
