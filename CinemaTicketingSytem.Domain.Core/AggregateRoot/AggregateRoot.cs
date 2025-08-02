using CinemaTicketingSystem.SharedKernel.Entities;

namespace CinemaTicketingSystem.SharedKernel.AggregateRoot;

public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


    public void AddDomainEvent(IDomainEvent eventData)
    {
        _domainEvents.Add(eventData);
    }
}

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }


    public void AddDomainEvent(IDomainEvent eventData)
    {
        _domainEvents.Add(eventData);
    }


}