#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.External;

public interface IScheduleQueryService
{
    Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId);
}