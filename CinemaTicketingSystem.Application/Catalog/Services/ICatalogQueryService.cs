#region

using CinemaTicketingSystem.Application.Abstraction;

#endregion

namespace CinemaTicketingSystem.Application.Catalog.Services;

public interface ICatalogQueryService
{
    Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid hallId, Guid movieId);
}