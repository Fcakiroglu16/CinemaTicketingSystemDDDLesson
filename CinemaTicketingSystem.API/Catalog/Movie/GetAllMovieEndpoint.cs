#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Movie;
using CinemaTicketingSystem.Presentation.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Movie;

internal static class GetAllMovieEndpoint
{
    public static RouteGroupBuilder GetAllMovieGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/movies",
                async ([FromServices] IMovieAppService movieAppService) =>
                (await movieAppService.GetAllAsync()).ToGenericResult())
            .WithName("GetAllMovies")
            .MapToApiVersion(1, 0);


        return group;
    }
}