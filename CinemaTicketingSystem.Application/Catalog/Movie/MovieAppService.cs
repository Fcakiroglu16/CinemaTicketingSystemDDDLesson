using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Movie;
using CinemaTicketingSystem.Application.Abstraction.Catalog.Movie.Create;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain;
using CinemaTicketingSystem.Domain.Catalog.Repositories;
using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Application.Catalog.Movie;

public class MovieAppService(IMovieRepository movieRepository, AppDependencyService appDependencyService)
    : IMovieAppService, IScopedDependency
{
    public async Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request)
    {
        var existMovie = await movieRepository.CheckIfMovieExists(request.Title);

        if (existMovie)
            return appDependencyService.Error<CreateMovieResponse>(ErrorCodes.MovieAlreadyExists, [request.Title]);

        var newMovie = new Domain.Catalog.Movie(request.Title, new Duration(request.Duration.TotalMinutes),
            request.PosterImageUrl);


        if (request.OriginalTitle is not null) newMovie.SetOriginalTitle(request.OriginalTitle);
        if (request.Description is not null) newMovie.SetDescription(request.Description);
        if (request.EarliestShowingDate.HasValue)
            newMovie.SetEarliestShowingDate(request.EarliestShowingDate.Value);

        await movieRepository.AddAsync(newMovie);
        await appDependencyService.UnitOfWork.SaveChangesAsync();
        return AppResult<CreateMovieResponse>.SuccessAsCreated(new CreateMovieResponse(newMovie.Id), "");
    }

    public async Task<AppResult<GetAllMovieResponse>> GetAllAsync()
    {
        var movies = await movieRepository.GetAllAsync();
        return AppResult<GetAllMovieResponse>.SuccessAsOk(new GetAllMovieResponse(movies.Select(movie => new MovieDto(
                movie.Id, movie.Title, movie.OriginalTitle, movie.PosterImageUrl, movie.Description,
                movie.Duration.Minutes,
                movie.Duration.GetFormattedDuration(), movie.SupportedTechnology))
            .ToList()));
    }
}