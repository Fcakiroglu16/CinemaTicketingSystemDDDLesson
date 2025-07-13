using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class DuplicateSeatException(SeatNumber seatNumber)
        : BusinessException($"Seat {seatNumber} cannot be added twice to the same purchase.", TicketingErrorCodes.DuplicateSeatErrorCode)
    {
        public SeatNumber SeatNumber { get; } = seatNumber;
    }
}
