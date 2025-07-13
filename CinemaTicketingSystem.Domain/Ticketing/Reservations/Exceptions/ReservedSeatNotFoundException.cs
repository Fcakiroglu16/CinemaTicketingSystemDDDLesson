using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class ReservedSeatNotFoundException(SeatNumber seatNumber)
    : BusinessException($"Reserved seat {seatNumber} was not found in this reservation.", TicketingErrorCodes.ReservedSeatNotFound)
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}