using Ardalis.GuardClauses;

namespace CinemaTicketingSystem.Domain.Scheduling;

public class ShowTime : ValueObject
{
    protected ShowTime()
    {
    }

    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public TimeSpan Duration => EndTime - StartTime;

    public static ShowTime Create(TimeOnly startTime, TimeOnly endTime)
    {
        Guard.Against.InvalidInput(startTime, nameof(startTime), x => x >= endTime,
            "Start time must be before end time");

        return new ShowTime
        {
            StartTime = startTime,
            EndTime = endTime
        };
    }

    public static ShowTime Create(TimeOnly startTime, Duration duration)
    {
        duration = Guard.Against.Null(duration, nameof(duration));
        Guard.Against.NegativeOrZero(duration.Minutes, nameof(duration), "Duration must be positive");

        var endTime = startTime.Add(duration.ToTimeSpan());

        return new ShowTime
        {
            StartTime = startTime,
            EndTime = endTime
        };
    }

    // Cinema Management Helper Methods

    /// <summary>
    ///     Checks if this showtime conflicts with another showtime (including cleaning time)
    /// </summary>
    public bool ConflictsWith(ShowTime other, int cleaningTime = 30)
    {
        other = Guard.Against.Null(other, nameof(other));
        Guard.Against.Negative(cleaningTime, nameof(cleaningTime));

        var thisEndTimeWithCleaning = EndTime.AddMinutes(cleaningTime);
        return thisEndTimeWithCleaning > other.StartTime;
    }

    /// <summary>
    ///     Checks if this is a morning showtime
    /// </summary>
    public bool IsMorningShow()
    {
        return StartTime.Hour is >= 9 and < 12;
    }

    /// <summary>
    ///     Checks if this showtime is suitable for children's movies
    /// </summary>
    public bool IsSuitableForChildren()
    {
        // Children's movies are usually before 8 PM
        return StartTime.Hour is >= 10 and <= 20;
    }

    /// <summary>
    ///     Checks if the showtime has started
    /// </summary>
    public bool HasStarted(TimeOnly currentTime)
    {
        return currentTime >= StartTime;
    }

    /// <summary>
    ///     Checks if the showtime has ended
    /// </summary>
    public bool HasEnded(TimeOnly currentTime)
    {
        return currentTime >= EndTime;
    }

    /// <summary>
    ///     Checks if the showtime duration is in the short film category
    /// </summary>
    public bool IsShortShow()
    {
        return Duration.TotalMinutes <= 90;
    }

    /// <summary>
    ///     Checks if the showtime duration is in the long film category
    /// </summary>
    public bool IsLongShow()
    {
        return Duration.TotalMinutes >= 180;
    }

    /// <summary>
    ///     Returns showtime information as a formatted string
    /// </summary>
    public string GetDisplayInfo()
    {
        return $"{StartTime:HH:mm} - {EndTime:HH:mm} ({Duration.TotalMinutes:F0} min)";
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return StartTime;
        yield return EndTime;
    }
}