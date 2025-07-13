using CinemaTicketingSystem.Domain.Ticketing.Tickets.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.DomainEvents;

public record TicketPurchasedEvent(Guid TicketId, Guid CustomerId, Price Price) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}