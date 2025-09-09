#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;

public record PurchaseCreatedIntegrationEvent(UserId userId, Guid TicketIssuanceId) : IIntegrationEvent;