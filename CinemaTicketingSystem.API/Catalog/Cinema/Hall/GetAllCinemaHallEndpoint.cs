#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Presentation.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Cinema.Hall;

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