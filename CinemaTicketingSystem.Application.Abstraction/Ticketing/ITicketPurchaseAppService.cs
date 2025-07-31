namespace CinemaTicketingSystem.Application.Abstraction.Ticketing;

public interface ITicketPurchaseAppService
{
    Task<AppResult> Purchase(PurchaseTicketRequest request);
}