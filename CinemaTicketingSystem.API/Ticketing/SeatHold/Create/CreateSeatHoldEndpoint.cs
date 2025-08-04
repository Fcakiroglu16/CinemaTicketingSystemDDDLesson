using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace CinemaTicketingSystem.API.Ticketing.SeatHold.Create;

public static class CreateSeatHoldEndpoint
{
    public static RouteGroupBuilder CreateSeatHoldGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/seathold",
                async (CreateSeatHoldRequest request, [FromServices] ISeatHoldAppService seatHoldAppService) =>
                (await seatHoldAppService.CreateSeatHoldAsync(request)).ToGenericResult())
            .WithName("CreateSeatHold")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateSeatHoldRequest>>();


        return group;
    }
}