namespace CinemaTicketingSystem.Domain.Ticketing
{
    internal class TicketPurchase : AggregateRoot<Guid>
    {
        public Guid? ReservedByCustomerId { get; private set; }
        public Guid MovieSessionId { get; private set; }

        private List<PurchasedTicket> _purchasedTickets { get; set; } = [];

        public IReadOnlyCollection<PurchasedTicket> PurchasedTickets => _purchasedTickets.AsReadOnly();

        public void Create(Guid movieSessionId, Guid customerId)
        {

            Id = Guid.CreateVersion7();
            MovieSessionId = movieSessionId;
            ReservedByCustomerId = customerId;

        }

        public void AddTicket(PurchasedTicket ticket)
        {
            if (_purchasedTickets.Any(t => t.SeatNumber == ticket.SeatNumber))
                throw new InvalidOperationException("Same seat cannot be added twice.");

            _purchasedTickets.Add(ticket);
        }
    }
}
