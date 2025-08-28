using CinemaTicketingSystem.Domain.BoundedContexts.Accounts.ValueObjects;
using CinemaTicketingSystem.Domain.BoundedContexts.Purchases.DomainEvents;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Purchases
{
    public class Purchase : AggregateRoot<Guid>
    {
        public UserId UserId { get; set; }

        public Price TotalPrice { get; set; }

        public Guid TicketIssuanceId { get; set; }

        public DateTime Created { get; set; }


        public Purchase(UserId userId, Price totalPrice, Guid ticketIssuanceId)
        {
            Id = Guid.CreateVersion7();
            UserId = userId;
            TotalPrice = totalPrice;
            TicketIssuanceId = ticketIssuanceId;
            Created = DateTime.UtcNow;


            AddIntegrationEvent(new PurchaseCreatedEvent(userId, ticketIssuanceId));
        }
    }
}