#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Create;

public static class CreateSeatHoldEndpoint
{
    public static RouteGroupBuilder CreateSeatHoldGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/seatholds",
                async (CreateSeatHoldRequest request, [FromServices] ISeatHoldAppService seatHoldAppService) =>
                (await seatHoldAppService.CreateAsync(request)).ToGenericResult())
            .WithName("CreateSeatHold")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateSeatHoldRequest>>();


        return group;
    }
}