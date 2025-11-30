#region

using CinemaTicketingSystem.Application.Contracts;
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
        Guid userId = appDependencyService.UserContext.UserId;
        //TODO : seat hold expire check can add here
        // purchase operation
        Domain.BoundedContexts.Purchases.Purchase purchase = new Domain.BoundedContexts.Purchases.Purchase(userId,
            new Price(request.Price.Amount, request.Price.Currency), request.TicketIssuanceId);

        await purchaseRepository.AddAsync(purchase);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult.SuccessAsNoContent();
    }

    Task<AppResult> IPurchaseAppService.Create(CreatePurchaseRequest request)
    {
        throw new NotImplementedException();
    }
}