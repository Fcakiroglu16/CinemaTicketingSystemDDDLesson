using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Movie.Create;

public static class CreateMovieEndpoint
{
    public static RouteGroupBuilder CreateMovieGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/item",
                async (CreateMovieRequest request, IMovieAppService movieAppService) =>
                    (await movieAppService.CreateAsync(request)).ToGenericResult())
            .WithName("CreateMovie")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateMovieRequest>>();


        return group;
    }
}