using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Reservations.Exceptions;

public class ReservationExpiredException(DateTime expirationTime)
    : BusinessException($"The reservation has expired at {expirationTime}.", TicketingErrorCodes.ReservationExpired)
{
    public DateTime ExpirationTime { get; } = expirationTime;
}