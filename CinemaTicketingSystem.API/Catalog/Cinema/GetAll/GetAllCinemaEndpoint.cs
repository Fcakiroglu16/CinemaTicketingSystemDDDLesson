using CinemaTicketingSystem.API.Localization;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;

namespace CinemaTicketingSystem.API.Catalog.Cinema.GetAll;

public static class GetAllCinemaEndpoint
{
    public static RouteGroupBuilder GetAllCinemaGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapGet("/cinemas",
                ([FromServices] ICinemaAppService cinemaAppService,
                    [FromServices] IStringLocalizer<SharedResource> stringLocalizer) =>
                {
                    // var result = (await cinemaAppService.GetAllAsync()).ToGenericResult();

                    var x = stringLocalizer["Hello"];
                })
            .WithName("GetAllCinema")
            .MapToApiVersion(1, 0);


        return group;
    }
}