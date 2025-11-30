#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.TicketIssuance.CreateFromReservation;

public static class CreateTicketIssuanceFromReservationTicketsEndpoint
{
    public static RouteGroupBuilder PurchaseTicketsFromReservationGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/reservation/{reservationId:guid}/issuance",
                async (Guid reservationId, [FromServices] ITicketPurchaseAppService purchaseAppService) =>
                (await purchaseAppService.CreateFromReservation(reservationId)).ToGenericResult())
            .WithName("TicketIssuanceFromReservation")
            .MapToApiVersion(1, 0);


        return group;
    }
}