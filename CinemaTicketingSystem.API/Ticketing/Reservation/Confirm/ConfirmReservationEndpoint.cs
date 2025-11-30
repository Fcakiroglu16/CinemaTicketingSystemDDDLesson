#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.Reservation.Confirm;

public static class ConfirmReservationEndpoint
{
    public static RouteGroupBuilder ConfirmReservationGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/reservation/confirm/{reservationId:guid}",
                async (Guid reservationId, [FromServices] IReservationAppService reservationAppService) =>
                (await reservationAppService.Confirm(reservationId)).ToGenericResult())
            .WithName("ConfirmReservation")
            .MapToApiVersion(1, 0);


        return group;
    }
}