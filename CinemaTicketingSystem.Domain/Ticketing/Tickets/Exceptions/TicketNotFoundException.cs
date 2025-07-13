using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;

public class TicketNotFoundException(SeatNumber seatNumber)
    : BusinessException($"Ticket for seat {seatNumber} not found in this purchase.", TicketingErrorCodes.TicketNotFound)
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}