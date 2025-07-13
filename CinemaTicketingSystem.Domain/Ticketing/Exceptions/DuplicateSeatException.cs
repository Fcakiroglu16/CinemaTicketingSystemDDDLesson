namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class DuplicateSeatException(SeatNumber seatNumber)
        : TicketPurchaseException($"Seat {seatNumber} cannot be added twice to the same purchase.")
    {
        public SeatNumber SeatNumber { get; } = seatNumber;
    }
}
