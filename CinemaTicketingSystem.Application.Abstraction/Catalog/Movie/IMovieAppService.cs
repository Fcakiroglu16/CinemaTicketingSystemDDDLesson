using CinemaTicketingSystem.Application.Abstraction.Catalog.Movie.Create;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;

namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Movie;

public interface IMovieAppService
{
    Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request);

    Task<AppResult<GetAllMovieResponse>> GetAllAsync();
}