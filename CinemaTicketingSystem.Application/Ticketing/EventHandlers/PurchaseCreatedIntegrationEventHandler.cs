#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance;
using CinemaTicketingSystem.Domain.BoundedContexts.Ticketing.Issuance.Specifications;
using CinemaTicketingSystem.Domain.Repositories;

#endregion

namespace CinemaTicketingSystem.Application.Ticketing.EventHandlers;

public class PurchaseCreatedIntegrationEventHandler(
    IUnitOfWork unitOfWork,
    ITicketIssuanceRepository ticketIssuanceRepository)
    : IIntegrationEventHandler<PurchaseCreatedIntegrationEvent>
{
    public async Task HandleAsync(PurchaseCreatedIntegrationEvent message,
        CancellationToken cancellationToken = default)
    {
        TicketIssuance ticketIssuance = await ticketIssuanceRepository.SingleAsync(
            new TicketIssuanceByCustomerAndIdSpec(message.userId, message.TicketIssuanceId));
        ticketIssuance.Confirm();
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}