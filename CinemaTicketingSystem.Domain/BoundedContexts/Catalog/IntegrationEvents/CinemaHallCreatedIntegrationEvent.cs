#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;

public record CinemaHallCreatedIntegrationEvent(Guid HallId, ScreeningTechnology hallTechnology, short SeatCount)
    : IIntegrationEvent
{
}