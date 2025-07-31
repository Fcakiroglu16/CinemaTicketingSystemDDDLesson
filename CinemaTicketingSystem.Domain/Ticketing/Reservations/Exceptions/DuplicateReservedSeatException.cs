using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Core.Exceptions;
using CinemaTicketingSystem.Domain.ValueObjects;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class DuplicateReservedSeatException
    : BusinessException
{
    public DuplicateReservedSeatException(SeatNumber seatNumber) : base(TicketingErrorCodes.DuplicateReservedSeat)
    {
        AddData(seatNumber.Row);
        AddData(seatNumber.Number.ToString());
    }
}