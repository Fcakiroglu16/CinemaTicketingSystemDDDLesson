#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Cinema.Create;

public static class CreateCinemaEndpoint
{
    public static RouteGroupBuilder CreateCinemaGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/cinemas",
                async (CreateCinemaRequest request, [FromServices] ICinemaAppService cinemaAppService) =>
                (await cinemaAppService.CreateAsync(request)).ToGenericResult())
            .WithName("CreateCinema")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateMovieRequest>>();


        return group;
    }
}