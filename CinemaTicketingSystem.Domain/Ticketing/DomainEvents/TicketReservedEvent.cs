namespace CinemaTicketingSystem.Domain.Ticketing.DomainEvents
{
    public record TicketReservedEvent(Guid TicketId, Guid CustomerId) : IDomainEvent
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }

}
