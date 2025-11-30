#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Movie;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Movie.Create;

public static class CreateMovieEndpoint
{
    public static RouteGroupBuilder CreateMovieGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/movies",
                async (CreateMovieRequest request, [FromServices] IMovieAppService movieAppService) =>
                (await movieAppService.CreateAsync(request)).ToGenericResult())
            .WithName("CreateMovie")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateMovieRequest>>();


        return group;
    }
}