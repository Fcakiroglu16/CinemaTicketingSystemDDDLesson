using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;
using System.Net;

namespace CinemaTicketingSystem.Application;

public class MovieAppService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    : IMovieAppService, IScopedDependency
{
    public async Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request)
    {

        var existMovie = await movieRepository.CheckIfMovieExists(request.Title);

        if (existMovie) return AppResult<CreateMovieResponse>.Error("Movie already exists", HttpStatusCode.BadRequest);

        var newMovie = new Movie(request.Title, new Duration(request.Duration.Minutes), request.PosterImageUrl);


        if (request.OriginalTitle is not null) newMovie.SetOriginalTitle(request.OriginalTitle);
        if (request.Description is not null) newMovie.SetDescription(request.Description);
        if (request.EarliestShowingDate.HasValue)
            newMovie.SetEarliestShowingDate(request.EarliestShowingDate.Value);

        await movieRepository.AddAsync(newMovie);
        await unitOfWork.SaveChangesAsync();
        return AppResult<CreateMovieResponse>.SuccessAsCreated(new CreateMovieResponse(newMovie.Id), "");
    }
}