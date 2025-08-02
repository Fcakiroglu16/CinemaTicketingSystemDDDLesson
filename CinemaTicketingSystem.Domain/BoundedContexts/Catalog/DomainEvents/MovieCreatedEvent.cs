using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;

public record MovieCreatedEvent(Guid MovieId, Duration Duration, ScreeningTechnology Technology) : IDomainEvent
{
}