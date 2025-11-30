#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface ITicketPurchaseAppService
{
    Task<AppResult<CreateTicketIssuanceResponse>> Create(CreateTicketIssuanceRequest request);

    Task<AppResult> CreateFromReservation(Guid reservationId);
}