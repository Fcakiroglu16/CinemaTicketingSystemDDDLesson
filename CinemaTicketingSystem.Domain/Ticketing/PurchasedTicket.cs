namespace CinemaTicketingSystem.Domain.Ticketing
{
    public class PurchasedTicket : Entity<Guid>
    {
        public SeatNumber SeatNumber { get; private set; }
        public Price Price { get; private set; }
        public string TicketCode { get; private set; }
        public bool IsUsed { get; private set; }
        public DateTime? UsedAt { get; private set; }
        internal PurchasedTicket(SeatNumber seatNumber, Price price)
        {
            Id = Guid.CreateVersion7();
            SeatNumber = seatNumber;
            Price = price;
            TicketCode = GenerateTicketCode();
            IsUsed = false;
        }


        public bool CanBeUsed()
        {
            return !IsUsed;
        }

        public string GetTicketInfo()
        {
            return $"Ticket: {TicketCode}, Seat: {SeatNumber}, Price: {Price}";
        }

        private static string GenerateTicketCode()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();

            return new string(Enumerable.Range(0, 6)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
    }
}
