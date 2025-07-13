using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class EmptyReservationException()
    : BusinessException("Cannot confirm a reservation with no seats.", TicketingErrorCodes.EmptyReservation)
{
}