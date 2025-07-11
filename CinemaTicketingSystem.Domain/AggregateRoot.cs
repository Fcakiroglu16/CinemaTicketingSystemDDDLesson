namespace CinemaTicketingSystem.Domain;

public class AggregateRoot<T> : Entity<T>
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<IDomainEvent> _integrationEvents = [];


    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public IReadOnlyCollection<IDomainEvent> IntegrationEvents => _integrationEvents.AsReadOnly();


    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public virtual void ClearIntegrationEvents()
    {
        _integrationEvents.Clear();
    }

    protected virtual void AddDomainEvent(IDomainEvent eventData)
    {
        _domainEvents.Add(eventData);
    }

    protected virtual void AddDistributedEvent(IDomainEvent eventData)
    {
        _integrationEvents.Add(eventData);
    }
}