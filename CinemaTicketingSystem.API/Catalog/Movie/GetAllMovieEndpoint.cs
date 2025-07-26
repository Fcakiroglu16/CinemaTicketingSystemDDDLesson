using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Catalog.Movie;

internal static class GetAllMovieEndpoint
{
    public static RouteGroupBuilder GetAllMovieGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/movies",
                async ([FromServices] ICinemaAppService cinemaAppService) =>
                (await cinemaAppService.GetAllAsync()).ToGenericResult())
            .WithName("GetAllMovies")
            .MapToApiVersion(1, 0);


        return group;
    }
}