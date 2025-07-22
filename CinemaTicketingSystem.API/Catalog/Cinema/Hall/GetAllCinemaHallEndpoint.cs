using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Catalog.Cinema.Hall;

public static class GetAllCinemaHallEndpoint
{
    public static RouteGroupBuilder GetAllCinemaHallGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/cinemas/{cinemaId:guid}/hall",
                async (Guid cinemaId, [FromServices] ICinemaAppService cinemaAppService) =>
                (await cinemaAppService.GetCinemaHallsAsync(cinemaId)).ToGenericResult())
            .WithName("GetAllCinemaHall")
            .MapToApiVersion(1, 0);


        return group;
    }
}