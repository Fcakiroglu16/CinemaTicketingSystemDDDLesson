using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class EmptyReservationException : DomainException
{
    public EmptyReservationException() : base(TicketingErrorCodes.EmptyReservation)
    {
    }
}