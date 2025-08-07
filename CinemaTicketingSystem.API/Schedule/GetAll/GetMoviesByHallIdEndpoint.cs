#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Contracts.Schedule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.API.Schedule.GetAll;

internal static class GetMoviesByHallIdEndpoint
{
    public static RouteGroupBuilder GetMoviesByHallIdGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/halls/{hallId:guid}/movies",
                async (Guid hallId, [FromServices] IScheduleAppService scheduleAppService) =>
                (await scheduleAppService.GetMoviesByHallId(hallId)).ToGenericResult())
            .WithName("GetMoviesByHallId")
            .MapToApiVersion(1, 0);


        return group;
    }
}