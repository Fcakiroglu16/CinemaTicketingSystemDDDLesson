namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents
{

    public record TicketCancelledEvent(Guid TicketId, Guid CustomerId) : IDomainEvent;
}
