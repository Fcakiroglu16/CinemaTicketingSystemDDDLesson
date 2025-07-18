using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.CinemaManagement.Cinema.Create;

public static class CreateCinemaEndpoint
{
    public static RouteGroupBuilder CreateCinemaGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/cinema",
                async (CreateCinemaRequest request, [FromServices] ICinemaAppService cinemaAppService) =>
                    (await cinemaAppService.CreateAsync(request)).ToGenericResult())
            .WithName("CreateCinema")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateMovieRequest>>();


        return group;
    }
}