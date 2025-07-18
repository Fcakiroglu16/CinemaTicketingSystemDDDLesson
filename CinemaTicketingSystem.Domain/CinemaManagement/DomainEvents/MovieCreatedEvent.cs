namespace CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents
{
    public record MovieCreatedEvent(Guid MovieId) : IDomainEvent
    {

    }
}
