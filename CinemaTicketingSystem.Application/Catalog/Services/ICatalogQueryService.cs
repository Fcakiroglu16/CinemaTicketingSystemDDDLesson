#region

#endregion

using CinemaTicketingSystem.Application.Contracts;

namespace CinemaTicketingSystem.Application.Catalog.Services;

public interface ICatalogQueryService
{
    Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid hallId, Guid movieId);
}