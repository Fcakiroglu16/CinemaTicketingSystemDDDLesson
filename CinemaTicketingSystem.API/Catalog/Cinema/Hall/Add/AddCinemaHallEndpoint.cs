using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema.Hall;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Catalog.Cinema.Hall.Add;

public static class AddCinemaHallEndpoint
{
    public static RouteGroupBuilder AddCinemaHallGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/cinemas/hall/add",
                async (AddCinemaHallRequest request, [FromServices] ICinemaAppService cinemaAppService) =>
                (await cinemaAppService.AddHallAsync(request)).ToGenericResult())
            .WithName("AddCinemaHall")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<AddCinemaHallValidator>>();


        return group;
    }
}