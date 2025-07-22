using CinemaTicketingSystem.Application.Abstraction;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.Domain.Scheduling;

namespace CinemaTicketingSystem.Application.Schedules;

public class MovieHallCompatibilityService : IDomainService
{
    /// <summary>
    /// Checks if the given movie is compatible with the specified cinema hall.
    /// </summary>
    public DomainResult IsCompatible(MovieSnapshot movie, CinemaHallSnapshot hall)
    {

        //Bu operatör, iki değerin ortak olan bitlerini bulur.Örneğin:
        //    •	hall.SupportedTechnologies = 3(Standard ve IMAX)
        //    •	movie.SupportedTechnology = 2(IMAX)
        //hall.SupportedTechnologies & movie.SupportedTechnology işlemi:
        //    •	3 & 2 = 2


        // Check if the hall supports the movie's required technology
        bool technologySupported = (hall.SupportedTechnologies & movie.SupportedTechnology) == movie.SupportedTechnology;


        //eğer movie IMAX ise koltuk sayısı 100'den az olamaz

        if (movie.SupportedTechnology.HasFlag(ScreeningTechnology.IMAX))
        {
            if (hall.SeatCount < 100)
            {

                return DomainResult.Failure("IMAX movies require at least 100 seats.");
            }

        }

        if (!technologySupported)
        {
            return DomainResult.Failure($"Hall does not support the movie's required technology.");
        }

        return DomainResult.Success();





    }
}