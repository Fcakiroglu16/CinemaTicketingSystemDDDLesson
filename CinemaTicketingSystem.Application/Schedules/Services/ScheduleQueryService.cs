#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling.Repositories;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.Logging;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Application.Schedules.Services;

public class ScheduleQueryService(
    IScheduleRepository scheduleRepository,
    AppDependencyService appDependencyService,
    ILogger<ScheduleQueryService> logger) : IScheduleQueryService, IScopedDependency
{
    public async Task<AppResult<GetScheduleInfoResponse>> GetScheduleInfo(Guid scheduleId)
    {
        Schedule? schedule = await scheduleRepository.GetByIdAsync(scheduleId);
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