namespace CinemaTicketingSystem.Domain
{
    public interface IAggregateRoot
    {

        IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
        void AddDomainEvent(IDomainEvent eventData);

    }
}
