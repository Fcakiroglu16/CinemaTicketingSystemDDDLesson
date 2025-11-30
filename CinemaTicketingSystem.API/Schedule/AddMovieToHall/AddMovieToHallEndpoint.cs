#region

using CinemaTicketingSystem.Application.Contracts.Schedule;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Schedule.AddMovieToHall;

public static class AddMovieToHallEndpoint
{
    public static RouteGroupBuilder AddMovieToHallGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/halls/{hallId:guid}/movies",
                async (Guid hallId, AddMovieToHallRequest request,
                        [FromServices] IScheduleAppService scheduleAppService) =>
                    (await scheduleAppService.AddMovieToHall(hallId, request)).ToGenericResult())
            .WithName("AddMovieToHall")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<AddMovieToHallValidator>>();


        return group;
    }
}