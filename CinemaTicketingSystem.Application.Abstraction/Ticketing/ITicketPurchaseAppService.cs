using CinemaTicketingSystem.Application.Abstraction;

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface ITicketPurchaseAppService
{
    Task<AppResult> Create(PurchaseTicketRequest request);

    Task<AppResult> CreateFromReservation(Guid reservationId);
}