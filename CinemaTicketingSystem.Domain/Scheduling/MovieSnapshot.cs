using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Scheduling;

public class MovieSnapshot : AggregateRoot<Guid>
{
    protected MovieSnapshot()
    {
    }

    public MovieSnapshot(Guid movieId, Duration duration,
        ScreeningTechnology supportedTechnology)
    {
        MovieId = movieId;
        Duration = duration;
        SupportedTechnology = supportedTechnology;
    }

    public Guid MovieId { get; set; }

    public Duration Duration { get; set; }

    public ScreeningTechnology SupportedTechnology { get; private set; } = ScreeningTechnology.Standard;
}