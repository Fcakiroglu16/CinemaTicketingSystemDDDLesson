#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using CinemaTicketingSystem.Presentation.API.Extensions;
using CinemaTicketingSystem.Presentation.API.Filters;
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
        group.MapPost("/reservation",
                async (CreateReservationRequest request, [FromServices] IReservationAppService reservationAppService) =>
                (await reservationAppService.Create(request)).ToGenericResult())
            .WithName("ReserveSeats")
            .MapToApiVersion(1, 0)
            .AddEndpointFilter<ValidationFilter<CreateReservationRequest>>();


        return group;
    }
}