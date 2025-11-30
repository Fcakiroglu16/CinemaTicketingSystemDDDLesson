#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.TicketIssuance.Create;

public static class TicketIssuanceEndpoint
{
    public static RouteGroupBuilder PurchaseTicketsGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/issuance",
                async (CreateTicketIssuanceRequest request,
                        [FromServices] ITicketPurchaseAppService purchaseAppService) =>
                    (await purchaseAppService.Create(request)).ToGenericResult())
            .WithName("TicketIssuance")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateReservationRequest>>();


        return group;
    }
}