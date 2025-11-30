#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.SharedKernel;
using MassTransit;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Messaging;

public class MassTransitConsumerAdapter<TMessage>(IIntegrationEventHandler<TMessage> handler) : IConsumer<TMessage>
    where TMessage : class, IIntegrationEvent
{
    public async Task Consume(ConsumeContext<TMessage> context)
    {
        await handler.HandleAsync(context.Message, context.CancellationToken);
    }
}