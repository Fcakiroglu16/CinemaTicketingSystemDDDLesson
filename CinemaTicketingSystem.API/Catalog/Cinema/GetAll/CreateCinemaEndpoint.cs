using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.CinemaManagement.Cinema.GetAll;

public static class GetAllCinemaEndpoint
{
    public static RouteGroupBuilder GetAllCinemaGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/cinemas",
                async ([FromServices] ICinemaAppService cinemaAppService) =>
                (await cinemaAppService.GetAllAsync()).ToGenericResult())
            .WithName("GetAllCinema")
            .MapToApiVersion(1, 0);


        return group;
    }
}