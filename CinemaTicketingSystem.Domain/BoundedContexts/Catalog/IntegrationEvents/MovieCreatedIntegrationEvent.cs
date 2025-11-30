#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.IntegrationEvents;

public record MovieCreatedIntegrationEvent(Guid MovieId, Duration Duration, ScreeningTechnology Technology)
    : IIntegrationEvent
{
}