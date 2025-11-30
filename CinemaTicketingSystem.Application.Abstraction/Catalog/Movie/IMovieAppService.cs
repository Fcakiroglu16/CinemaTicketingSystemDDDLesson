#region

#endregion

using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;

namespace CinemaTicketingSystem.Application.Contracts.Catalog.Movie;

public interface IMovieAppService
{
    Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request);

    Task<AppResult<GetAllMovieResponse>> GetAllAsync();
}