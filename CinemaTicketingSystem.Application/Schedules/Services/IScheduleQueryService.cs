#region

#endregion

using CinemaTicketingSystem.Application.Contracts;

namespace CinemaTicketingSystem.Application.Schedules.Services;

public interface IScheduleQueryService
{
    Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId);
}