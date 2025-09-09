#region

using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases;

public class Purchase : AggregateRoot<Guid>
{
    protected Purchase()
    {
    }

    public Purchase(UserId userId, Price totalPrice, Guid ticketIssuanceId)
    {
        Id = Guid.CreateVersion7();
        UserId = userId;
        TotalPrice = totalPrice;
        TicketIssuanceId = ticketIssuanceId;
        Created = DateTime.UtcNow;


        AddIntegrationEvent(new PurchaseCreatedIntegrationEvent(userId, ticketIssuanceId));
    }

    public UserId UserId { get; set; }

    public Price TotalPrice { get; set; }

    public Guid TicketIssuanceId { get; set; }

    public DateTime Created { get; set; }
}