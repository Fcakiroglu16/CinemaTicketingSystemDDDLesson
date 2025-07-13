namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class TicketNotFoundException(SeatNumber seatNumber)
        : TicketPurchaseException($"Ticket for seat {seatNumber} not found in this purchase.")
    {
        public SeatNumber SeatNumber { get; } = seatNumber;
    }
}
