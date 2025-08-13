#region

using CinemaTicketingSystem.API.Extensions;
using CinemaTicketingSystem.API.Filters;
using CinemaTicketingSystem.Application.Contracts.Ticketing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.Reservation.Reserve;

public static class ReserveSeatsEndpoint
{
    public static RouteGroupBuilder ReserveSeatsGroupItemEndpoint(this RouteGroupBuilder group)
    {
        group.MapPost("/reservations",
                async (ReserveSeatsRequest request, [FromServices] IReservationAppService reservationAppService) =>
                (await reservationAppService.Create(request)).ToGenericResult())
            .WithName("ReserveSeats")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<ReserveSeatsRequest>>();


        return group;
    }
}