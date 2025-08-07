#region

using System.Net;
using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Catalog.ICL.Dto;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.Logging;

#endregion

namespace CinemaTicketingSystem.Application.Catalog.ICL;

public class CatalogQueryService(
    ICinemaRepository cinemaRepository,
    IMovieRepository movieRepository,
    AppDependencyService appDependencyService,
    ILogger<CatalogQueryService> logger) : ICatalogQueryService, IScopedDependency
{
    public async Task<AppResult<GetCatalogInfoResponse>> GetCinemaInfo(Guid hallId, Guid movieId)
    {
        var movie = await movieRepository.GetByIdAsync(movieId);

        if (movie is null)
        {
            logger.LogWarning($"Movie with Id: {movieId} not found.");
            return appDependencyService.LocalizeError.Error<GetCatalogInfoResponse>(ErrorCodes.MovieNotFound,
                HttpStatusCode.NotFound);
        }

        var cinema = await cinemaRepository.GetByHallId(hallId);

        if (cinema is null)
        {
            logger.LogWarning($"Cinema with Hall Id: {hallId} not found.");
            return appDependencyService.LocalizeError.Error<GetCatalogInfoResponse>(ErrorCodes.CinemaNotFound,
                HttpStatusCode.NotFound);
        }

        var hall = cinema.Halls.FirstOrDefault(h => h.Id == hallId);

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