using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Ticketing;

namespace CinemaTicketingSystem.Application.Ticketing;

public interface ITicketingAppService
{
    Task<AppResult> PurchaseTicket(PurchaseTicketRequest request);
}