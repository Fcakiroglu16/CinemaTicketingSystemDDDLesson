#region

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

    public Purchase(PayerId payerId, Price totalPrice, Guid ticketIssuanceId)
    {
        Id = Guid.CreateVersion7();
        PayerId = payerId;
        TotalPrice = totalPrice;
        TicketIssuanceId = ticketIssuanceId;
        Created = DateTime.UtcNow;


        AddIntegrationEvent(new PurchaseCreatedIntegrationEvent(payerId, ticketIssuanceId));
    }

    public PayerId PayerId { get; set; }

    public Price TotalPrice { get; set; }

    public Guid TicketIssuanceId { get; set; }

    public DateTime Created { get; set; }
}