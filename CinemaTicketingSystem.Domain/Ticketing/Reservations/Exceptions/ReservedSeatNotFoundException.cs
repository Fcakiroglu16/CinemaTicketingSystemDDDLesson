using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Ticketing.Tickets.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class ReservedSeatNotFoundException(SeatNumber seatNumber)
    : BusinessException($"Reserved seat {seatNumber} was not found in this reservation.", "Reservation.SeatNotFound")
{
    public SeatNumber SeatNumber { get; } = seatNumber;
}