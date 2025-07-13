namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public abstract class TicketPurchaseException : Exception
    {
        protected TicketPurchaseException(string message) : base(message) { }
        protected TicketPurchaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
