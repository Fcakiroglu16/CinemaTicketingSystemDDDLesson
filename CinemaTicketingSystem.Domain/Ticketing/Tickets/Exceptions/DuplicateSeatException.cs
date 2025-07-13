using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;

public class DuplicateSeatException(SeatNumber seatNumber)
    : BusinessException($"Seat {seatNumber} cannot be added twice to the same purchase.",
        TicketingErrorCodes.DuplicateSeatErrorCode)
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}