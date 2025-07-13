namespace CinemaTicketingSystem.Domain.Ticketing.Exceptions
{
    public class MaxTicketLimitExceededException(int maxAllowed)
        : TicketPurchaseException($"Cannot purchase more than {maxAllowed} tickets at once.")
    {
        public int MaxAllowed { get; } = maxAllowed;
    }
}
