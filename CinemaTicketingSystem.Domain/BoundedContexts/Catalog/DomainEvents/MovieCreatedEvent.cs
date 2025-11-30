#region

using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;

public record MovieCreatedEvent(Guid MovieId, Duration Duration, ScreeningTechnology Technology) : IDomainEvent
{
}