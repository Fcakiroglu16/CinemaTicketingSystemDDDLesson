#region

using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;

public record PurchaseCreatedIntegrationEvent(PayerId userId, Guid TicketIssuanceId) : IIntegrationEvent;