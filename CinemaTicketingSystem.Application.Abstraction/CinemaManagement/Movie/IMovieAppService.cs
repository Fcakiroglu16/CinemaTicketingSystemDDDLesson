using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;

namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie;

public interface IMovieAppService
{

    Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request);


}