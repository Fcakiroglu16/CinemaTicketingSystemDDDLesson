using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain.Catalog;

namespace CinemaTicketingSystem.Application;

public class CinemaMovieDomainService : IDomainService
{
    public DomainResult CanMovieBeAssignedToHall(Movie movie, CinemaHall hall)
    {
        //if (!hall.IsOperational)
        //{
        //    return DomainResult.Failure("Hall is not operational");
        //}

        //if (hall.MovieId.HasValue)
        //{
        //    return DomainResult.Failure("Hall is already assigned to a movie");
        //}

        //var result = hall.CanShowMovie(movie.SupportedTechnology);

        //if (!result)
        //{
        //    return DomainResult.Failure("Hall is already assigned to a movie");

        //}


        return DomainResult.Success();
    }
}