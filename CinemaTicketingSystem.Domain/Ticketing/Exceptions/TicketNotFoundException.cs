using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class TicketNotFoundException(SeatNumber seatNumber)
        : BusinessException($"Ticket for seat {seatNumber} not found in this purchase.", TicketingErrorCodes.TicketNotFound)
    {
        public SeatNumber SeatNumber { get; } = seatNumber;
    }
}
