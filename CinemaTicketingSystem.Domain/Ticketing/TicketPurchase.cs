using CinemaTicketingSystem.Domain.Ticketing.DomainEvents;

namespace CinemaTicketingSystem.Domain.Ticketing
{
    internal class TicketPurchase : AggregateRoot<Guid>
    {
        // Existing properties
        public Guid? CustomerId { get; private set; }
        public Guid MovieSessionId { get; private set; }
        public bool IsDiscountApplied { get; private set; }

        private List<PurchasedTicket> _purchasedTickets { get; set; } = [];

        public IReadOnlyCollection<PurchasedTicket> PurchasedTickets => _purchasedTickets.AsReadOnly();


        private const int MaxTicketsPerPurchase = 10;

        public void Create(Guid movieSessionId, Guid customerId)
        {
            Id = Guid.CreateVersion7();
            MovieSessionId = movieSessionId;
            CustomerId = customerId;
        }

        public void AddTicket(PurchasedTicket ticket)
        {

            if (_purchasedTickets.Count >= MaxTicketsPerPurchase)
                throw new InvalidOperationException($"Cannot purchase more than {MaxTicketsPerPurchase} tickets at once.");

            if (_purchasedTickets.Any(t => t.SeatNumber == ticket.SeatNumber))
                throw new InvalidOperationException("Same seat cannot be added twice.");

            _purchasedTickets.Add(ticket);
            ApplyBulkDiscountIfEligible();
            AddDomainEvent(new TicketPurchasedEvent(ticket.Id, CustomerId!.Value, ticket.Price));
        }
        public void RemoveTicket(SeatNumber seatNumber)
        {
            var ticket = _purchasedTickets.FirstOrDefault(t => t.SeatNumber == seatNumber);
            if (ticket == null)
                throw new InvalidOperationException("Ticket not found.");

            _purchasedTickets.Remove(ticket);
            AddDomainEvent(new TicketReleasedEvent(ticket.Id));

            ApplyBulkDiscountIfEligible();
        }

        public void AddTickets(IEnumerable<PurchasedTicket> tickets)
        {
            foreach (var ticket in tickets)
            {
                AddTicket(ticket);
            }
            ApplyBulkDiscountIfEligible();
        }




        private void ApplyBulkDiscountIfEligible()
        {
            if (_purchasedTickets.Count >= 3 && !IsDiscountApplied)
            {

                IsDiscountApplied = true;

            }
        }

        public Price GetTotalPrice()
        {
            var baseTotal = _purchasedTickets
                .Select(t => t.Price)
                .Aggregate((total, next) => total + next);

            if (IsDiscountApplied)
            {
                // Apply 10% discount for bulk purchases
                decimal discountMultiplier = 0.9m; // 10% off
                return new Price(baseTotal.Amount * discountMultiplier, baseTotal.Currency);
            }

            return baseTotal;
        }

        public void MarkTicketsAsUsed()
        {
            foreach (var ticket in _purchasedTickets)
            {
                ticket.MarkAsUsed();
                AddDomainEvent(new TicketUsedEvent(ticket.Id, CustomerId!.Value, DateTime.UtcNow));
            }
        }

        public bool HasTicketForSeat(SeatNumber seatNumber)
        {
            return _purchasedTickets.Any(t => t.SeatNumber == seatNumber);
        }


    }
}
