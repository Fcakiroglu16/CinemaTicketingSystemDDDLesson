using Asp.Versioning.Builder;
using CinemaTicketingSystem.API.Ticketing.Purchase.Purchase;
using CinemaTicketingSystem.API.Ticketing.Reservation.Reserve;
using CinemaTicketingSystem.API.Ticketing.SeatHold.Create;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace CinemaTicketingSystem.API.Ticketing;

public static class TicketingEndpointExt
{
    public static void AddTicketingGroupEndpointExt(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        app.MapGroup("api/v{version:apiVersion}/ticketing").WithTags("ticketing")
            .WithApiVersionSet(apiVersionSet)
            .CreateSeatHoldGroupItemEndpoint()
            .ReserveSeatsGroupItemEndpoint()
            .PurchaseTicketsGroupItemEndpoint().RequireAuthorization();
    }
}