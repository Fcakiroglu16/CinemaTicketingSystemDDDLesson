using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Abstraction.Schedule;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Schedule;

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