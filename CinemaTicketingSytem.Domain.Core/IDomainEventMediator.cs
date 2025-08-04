namespace CinemaTicketingSystem.SharedKernel
{
    public interface IDomainEventMediator
    {
        Task PublishAsync<T>(T domainEvent, CancellationToken cancellationToken = default) where T : IDomainEvent;
    }
}
