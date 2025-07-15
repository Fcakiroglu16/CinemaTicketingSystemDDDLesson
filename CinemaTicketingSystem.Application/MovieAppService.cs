using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie;
using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using CinemaTicketingSystem.Application.Abstraction.DependencyInjections;
using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Repositories;

namespace CinemaTicketingSystem.Application
{
    internal class MovieAppService(IMovieRepository movieRepository, IUnitOfWork unitOfWork) : IScopedDependency, IMovieAppService
    {



        public async Task<AppResult<CreateMovieResponse>> CreateAsync(CreateMovieRequest request)
        {


            var newMovie = new Movie(request.Title, new Duration(request.Duration.Minutes));


            if (request.OriginalTitle is not null) newMovie.SetOriginalTitle(request.OriginalTitle);
            if (request.Description is not null) newMovie.SetDescription(request.Description);
            if (request.EarliestShowingDate.HasValue)
                newMovie.SetEarliestShowingDate(request.EarliestShowingDate.Value);

            await movieRepository.AddAsync(newMovie);
            await unitOfWork.SaveChangesAsync();
            return AppResult<CreateMovieResponse>.SuccessAsCreated(new CreateMovieResponse(newMovie.Id), "");

        }





    }
}
