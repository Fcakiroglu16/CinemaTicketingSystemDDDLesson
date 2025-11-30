#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;
using CinemaTicketingSystem.SharedKernel;

#endregion

namespace CinemaTicketingSystem.Application.Schedules;

public class MovieHallCompatibilityService : IDomainService
{
    private const int MinimumIMAXSeatCount = 100;

    /// <summary>
    ///     Checks if the given movie is compatible with the specified cinema hall.
    /// </summary>
    public DomainResult IsCompatible(MovieSnapshot movie, CinemaHallSnapshot hall)
    {
        //    •	hall.SupportedTechnologies = 3(Standard ve IMAX)
        //    •	movie.SupportedTechnology = 2(IMAX)
        //hall.SupportedTechnologies & movie.SupportedTechnology işlemi:
        //    •	3 & 2 = 2


        // Check if the hall supports the movie's required technology
        bool technologySupported = (hall.SupportedTechnologies & movie.SupportedTechnology) == movie.SupportedTechnology;


        if (movie.SupportedTechnology.HasFlag(ScreeningTechnology.IMAX))
            if (hall.SeatCount < MinimumIMAXSeatCount)
                return DomainResult.Failure(ErrorCodes.ImaxRequiresMinimumSeats, MinimumIMAXSeatCount);

        return !technologySupported
            ? DomainResult.Failure(ErrorCodes.HallTechnologyNotSupported)
            : DomainResult.Success();
    }
}