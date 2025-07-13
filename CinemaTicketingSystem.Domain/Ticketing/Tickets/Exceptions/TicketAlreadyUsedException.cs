using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Tickets.Exceptions;

public class TicketAlreadyUsedException(string ticketCode) : BusinessException(
    $"Ticket {ticketCode} has already been used and cannot be used again.",
    TicketingErrorCodes.TicketAlreadyUsed)
{
    public string TicketCode { get; } = ticketCode;
}