#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Specifications;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.Logging;
using System.Net;

#endregion

namespace CinemaTicketingSystem.Application.Catalog.Services;

public class CatalogQueryService(
    ICinemaRepository cinemaRepository,
    IMovieRepository movieRepository,
    AppDependencyService appDependencyService,
    ILogger<CatalogQueryService> logger) : ICatalogQueryService, IScopedDependency
{
    public async Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid hallId, Guid movieId)
    {
        Domain.BoundedContexts.Catalog.Movie? movie = await movieRepository.GetByIdAsync(movieId);

        if (movie is null)
        {
            logger.LogWarning($"Movie with Id: {movieId} not found.");
            return appDependencyService.LocalizeError.Error<GetCatalogInfoResponse>(ErrorCodes.MovieNotFound,
                HttpStatusCode.NotFound);
        }

        Domain.BoundedContexts.Catalog.Cinema? cinema = await cinemaRepository.FirstOrDefaultAsync(
            new CinemaByHallIdSpec(hallId));

        if (cinema is null)
        {
            logger.LogWarning($"Cinema with Hall Id: {hallId} not found.");
            return appDependencyService.LocalizeError.Error<GetCatalogInfoResponse>(ErrorCodes.CinemaNotFound,
                HttpStatusCode.NotFound);
        }

        CinemaHall? hall = cinema.Halls.FirstOrDefault(h => h.Id == hallId);

        if (hall is null)
        {
            logger.LogWarning($"Cinema hall with Id: {hallId} not found in cinema with Name: {cinema.Name}.");
            return appDependencyService.LocalizeError.Error<GetCatalogInfoResponse>(ErrorCodes.CinemaHallNotFound,
                HttpStatusCode.NotFound);
        }


        return AppResult<GetCatalogInfoResponse>.SuccessAsOk(new GetCatalogInfoResponse(cinema.Name, hall.Name,
            movie.Title, hall.Capacity));
    }
}