#region

#endregion

namespace CinemaTicketingSystem.Application.Contracts.Purchase;

public interface IPurchaseAppService
{
    Task<AppResult> Create(CreatePurchaseRequest request);
}