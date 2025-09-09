#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.Services;

public interface IScheduleQueryService
{
    Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId);
}