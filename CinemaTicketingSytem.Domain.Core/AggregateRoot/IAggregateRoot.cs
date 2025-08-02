namespace CinemaTicketingSystem.SharedKernel.AggregateRoot;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
    void AddDomainEvent(IDomainEvent eventData);
}