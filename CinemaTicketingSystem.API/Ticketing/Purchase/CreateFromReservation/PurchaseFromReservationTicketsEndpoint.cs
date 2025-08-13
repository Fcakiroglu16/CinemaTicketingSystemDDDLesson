#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.Purchase.CreateFromReservation;

public static class PurchaseFromReservationTicketsEndpoint
{
    public static RouteGroupBuilder PurchaseTicketsFromReservationGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/reservations/{reservationId:guid}/purchase",
                async (Guid reservationId, [FromServices] ITicketPurchaseAppService purchaseAppService) =>
                (await purchaseAppService.CreateFromReservation(reservationId)).ToGenericResult())
            .WithName("purchaseFromReservation")
            .MapToApiVersion(1, 0);


        return group;
    }
}