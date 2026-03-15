#region

using CinemaTicketingSystem.Application.Contracts;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie;
using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.Application.Contracts.DependencyInjections;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Repositories;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.Specifications;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Application.Catalog.Movie;

public class MovieAppService(IMovieRepository movieRepository, AppDependencyService appDependencyService)
    : IMovieAppService, IScopedDependency
{
    public async Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request)
    {
        bool existMovie = await movieRepository.AnyAsync(new MovieByTitleSpec(request.Title));

        if (existMovie)
            return appDependencyService.LocalizeError.Error<CreateMovieResponse>(ErrorCodes.MovieAlreadyExists,
                [request.Title]);


        Domain.BoundedContexts.Catalog.Movie newMovie = new Domain.BoundedContexts.Catalog.Movie(request.Title,
            new Duration(request.Duration.TotalMinutes),
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
        IEnumerable<Domain.BoundedContexts.Catalog.Movie> movies = await movieRepository.GetAllAsync();
        return AppResult<GetAllMovieResponse>.SuccessAsOk(new GetAllMovieResponse(movies.Select(movie => new MovieDto(
                movie.Id, movie.Title, movie.OriginalTitle, movie.PosterImageUrl, movie.Description,
                movie.Duration.Minutes,
                movie.Duration.GetFormattedDuration(), movie.SupportedTechnology))
            .ToList()));
    }
}