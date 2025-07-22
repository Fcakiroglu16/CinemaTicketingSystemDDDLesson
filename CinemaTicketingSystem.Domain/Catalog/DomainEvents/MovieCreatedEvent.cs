using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Catalog.DomainEvents
{
    public record MovieCreatedEvent(Guid MovieId, Duration Duration, ScreeningTechnology Technology) : IDomainEvent
    {

    }
}
