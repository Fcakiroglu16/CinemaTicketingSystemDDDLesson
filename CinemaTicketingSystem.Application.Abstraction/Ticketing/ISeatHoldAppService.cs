#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Ticketing;

public interface ISeatHoldAppService
{
    Task<AppResult> CreateAsync(CreateSeatHoldRequest request);
    Task<AppResult> Cancel();
    Task<AppResult> ConfirmAsync(ConfirmSeatHoldRequest request);
}