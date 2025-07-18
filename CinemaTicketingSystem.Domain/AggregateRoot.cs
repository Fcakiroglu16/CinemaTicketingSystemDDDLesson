namespace CinemaTicketingSystem.Domain;

public class AggregateRoot<T> : Entity<T>, IAggregateRoot
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