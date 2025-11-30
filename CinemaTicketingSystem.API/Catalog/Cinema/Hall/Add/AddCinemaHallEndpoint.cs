#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema;
using CinemaTicketingSystem.Application.Contracts.Catalog.Cinema.Hall;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Cinema.Hall.Add;

public static class AddCinemaHallEndpoint
{
    public static RouteGroupBuilder AddCinemaHallGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/cinemas/{cinemaId:guid}/hall",
                async (AddCinemaHallRequest request, Guid cinemaId,
                        [FromServices] ICinemaAppService cinemaAppService) =>
                    (await cinemaAppService.AddHallAsync(cinemaId, request)).ToGenericResult())
            .WithName("AddCinemaHall")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<AddCinemaHallValidator>>();


        return group;
    }
}