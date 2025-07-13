using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class DuplicateReservedSeatException(SeatNumber seatNumber)
    : BusinessException($"Seat {seatNumber} cannot be reserved twice in the same reservation.",
        TicketingErrorCodes.DuplicateReservedSeat)
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}