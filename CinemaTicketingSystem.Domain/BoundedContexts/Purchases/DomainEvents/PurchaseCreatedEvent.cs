using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents
{
    public record PurchaseCreatedEvent(UserId userId, Guid TicketIssuanceId) : IIntegrationEvent;
}