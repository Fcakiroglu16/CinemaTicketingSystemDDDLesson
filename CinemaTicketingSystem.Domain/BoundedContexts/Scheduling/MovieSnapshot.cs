#region

using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

#endregion

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;

public class MovieSnapshot : AggregateRoot<Guid>
{
    protected MovieSnapshot()
    {
    }

    public MovieSnapshot(Guid movieId, Duration duration,
        ScreeningTechnology supportedTechnology)
    {
        Id = Guard.Against.Default(movieId, nameof(movieId));
        Duration = Guard.Against.Null(duration, nameof(duration));
        SupportedTechnology = supportedTechnology;
    }

    public Duration Duration { get; set; }

    public ScreeningTechnology SupportedTechnology { get; private set; } = ScreeningTechnology.Standard;

    public bool IsValidDuration(TimeOnly startTime, TimeOnly endTime, int toleranceMinutes = 0)
    {
        Guard.Against.InvalidInput(startTime, nameof(startTime), x => x >= endTime,
            "Start time must be before end time");

        Guard.Against.Negative(toleranceMinutes, nameof(toleranceMinutes));

        TimeSpan showDuration = endTime - startTime;
        TimeSpan movieDurationTimeSpan = Duration.ToTimeSpan();

        double difference = Math.Abs((showDuration - movieDurationTimeSpan).TotalMinutes);

        return difference <= toleranceMinutes;
    }
}