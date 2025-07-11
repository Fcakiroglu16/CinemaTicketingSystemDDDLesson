namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents
{
    public record TicketPurchasedEvent(Guid TicketId, Guid CustomerId, Price Price) : IDomainEvent
    {

        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
