#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Cancel;

public static class CancelSeatHoldEndpoint
{
    public static RouteGroupBuilder CancelSeatHoldGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/seatholds/cancel",
                async ([FromServices] ISeatHoldAppService seatHoldAppService) =>
                (await seatHoldAppService.Cancel()).ToGenericResult())
            .WithName("CancelSeatHold")
            .MapToApiVersion(1, 0);


        return group;
    }
}