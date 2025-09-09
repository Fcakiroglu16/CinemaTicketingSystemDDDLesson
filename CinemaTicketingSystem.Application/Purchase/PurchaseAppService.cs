#region

using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Application.Contracts.Purchase;
using CinemaTicketingSystem.Domain.Repositories;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Purchase;

public class PurchaseAppService(
    AppDependencyService appDependencyService,
    IGenericRepository<Domain.BoundedContexts.Purchases.Purchase> purchaseRepository)
    : IScopedDependency, IPurchaseAppService
{
    public async Task<AppResult> Create(CreatePurchaseRequest request)
    {
        var userId = appDependencyService.UserContext.UserId;

        // purchase operation
        var purchase = new Domain.BoundedContexts.Purchases.Purchase(userId,
            new Price(request.Price.Amount, request.Price.Currency), request.TicketIssuanceId);

        await purchaseRepository.AddAsync(purchase);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }
}