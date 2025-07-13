using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class TicketAlreadyUsedException : BusinessException
    {
        public string TicketCode { get; }

        public TicketAlreadyUsedException(string ticketCode)
            : base($"Ticket {ticketCode} has already been used and cannot be used again.", TicketingErrorCodes.TicketAlreadyUsed)
        {
            TicketCode = ticketCode;
        }
    }
}