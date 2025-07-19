namespace CinemaTicketingSystem.Application.Contracts
{
    public interface IIntegrationEventBus
    {

        Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default) where T : class;
    }
}
