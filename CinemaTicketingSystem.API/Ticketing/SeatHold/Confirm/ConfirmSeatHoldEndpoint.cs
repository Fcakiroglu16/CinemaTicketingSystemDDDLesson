#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Confirm;

public static class ConfirmSeatHoldEndpoint
{
    public static RouteGroupBuilder confirmSeatHoldGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/seatholds/confirm",
                async (ConfirmSeatHoldRequest request, [FromServices] ISeatHoldAppService seatHoldAppService) =>
                (await seatHoldAppService.ConfirmAsync(request)).ToGenericResult())
            .WithName("ConfirmSeatHold")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<ConfirmSeatHoldRequest>>();


        return group;
    }
}