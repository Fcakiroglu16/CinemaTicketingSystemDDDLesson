#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface IReservationAppService
{
    Task<AppResult<CreateReservationResponse>> Create(CreateReservationRequest request);
    Task<AppResult> Confirm(Guid reservationId);
}