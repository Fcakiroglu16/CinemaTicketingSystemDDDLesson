using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Scheduling.Repositories;
using Microsoft.Extensions.Logging;
using System.Net;

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public class ScheduleQueryService(
    IScheduleRepository scheduleRepository,
    AppDependencyService appDependencyService,
    ILogger<ScheduleQueryService> logger) : IScheduleQueryService
{
    public async Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId)
    {
        var schedule = await scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule is null)
        {
            logger.LogWarning("Schedule with Id {scheduleId} was not found", scheduleId);
            return appDependencyService.Error<GetScheduleInfoResponse>(ErrorCodes.ScheduleNotFound,
                HttpStatusCode.NotFound);
        }

        return AppResult<GetScheduleInfoResponse>.SuccessAsOk(new GetScheduleInfoResponse(schedule!.HallId,
            schedule.MovieId, schedule.ShowTime, schedule.TicketPrice));
    }
}