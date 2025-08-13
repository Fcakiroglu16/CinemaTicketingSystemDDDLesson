#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.Purchase.Create;

public static class PurchaseTicketsEndpoint
{
    public static RouteGroupBuilder PurchaseTicketsGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/purchases",
                async (PurchaseTicketRequest request, [FromServices] ITicketPurchaseAppService purchaseAppService) =>
                (await purchaseAppService.Create(request)).ToGenericResult())
            .WithName("purchaseTickets")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<ReserveSeatsRequest>>();


        return group;
    }
}