using CinemaTicketingSystem.SharedKernel.Entities;

namespace CinemaTicketingSystem.SharedKernel.AggregateRoot;


public abstract class AggregateRootBase : EntityBase
{
    private readonly List<IDomainEvent> _domainEvents = [];
    private readonly List<IIntegrationEvent> _integrationEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public IReadOnlyCollection<IIntegrationEvent> IntegrationEvents => _integrationEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();
    public void ClearIntegrationEvents() => _integrationEvents.Clear();
    public void AddDomainEvent(IDomainEvent eventData) => _domainEvents.Add(eventData);
    public void AddIntegrationEvent(IIntegrationEvent eventData) => _integrationEvents.Add(eventData);
}

public abstract class AggregateRoot<T> : AggregateRootBase, IAggregateRoot where T : notnull
{
    public T Id { get; protected set; } = default!;

    protected override object?[] GetKeys()
    {
        return [Id];
    }
}

public abstract class AggregateRoot : AggregateRootBase, IAggregateRoot
{
    protected abstract override object?[] GetKeys();
}