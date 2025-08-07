#region

using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.Logging;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.ICL;

public class ScheduleQueryService(
    IScheduleRepository scheduleRepository,
    AppDependencyService appDependencyService,
    ILogger<ScheduleQueryService> logger) : IScheduleQueryService, IScopedDependency
{
    public async Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId)
    {
        var schedule = await scheduleRepository.GetByIdAsync(scheduleId);
        if (schedule is null)
        {
            logger.LogWarning("Schedule with Id {scheduleId} was not found", scheduleId);
            return appDependencyService.LocalizeError.Error<GetScheduleInfoResponse>(ErrorCodes.ScheduleNotFound,
                HttpStatusCode.NotFound);
        }

        return AppResult<GetScheduleInfoResponse>.SuccessAsOk(new GetScheduleInfoResponse(schedule!.HallId,
            schedule.MovieId, schedule.ShowTime, schedule.TicketPrice));
    }
}