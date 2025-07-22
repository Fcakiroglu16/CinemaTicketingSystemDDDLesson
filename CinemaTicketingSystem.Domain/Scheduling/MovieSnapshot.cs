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

        Id = movieId;
        Duration = duration;
        SupportedTechnology = supportedTechnology;
    }



    public Duration Duration { get; set; }

    public ScreeningTechnology SupportedTechnology { get; private set; } = ScreeningTechnology.Standard;


    public bool IsValidDuration(TimeOnly startTime, TimeOnly endTime, int toleranceMinutes = 0)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        var showDuration = endTime - startTime;
        var movieDurationTimeSpan = Duration.ToTimeSpan();

        var difference = Math.Abs((showDuration - movieDurationTimeSpan).TotalMinutes);

        return difference <= toleranceMinutes;
    }

}