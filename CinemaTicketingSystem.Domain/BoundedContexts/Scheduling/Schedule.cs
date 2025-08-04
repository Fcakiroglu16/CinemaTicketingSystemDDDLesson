using Ardalis.GuardClauses;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;
using CinemaTicketingSystem.SharedKernel.ValueObjects;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Scheduling;

public class Schedule : AggregateRoot<Guid>
{
    protected Schedule()
    {
    }

    public Schedule(Guid movieId, Guid hallId, ShowTime showTime, Price price)
    {
        Guard.Against.Default(movieId, nameof(movieId));
        Guard.Against.Default(hallId, nameof(hallId));
        ShowTime = Guard.Against.Null(showTime, nameof(showTime));

        TicketPrice = Guard.Against.Null(price, nameof(price));
        MovieId = movieId;
        HallId = hallId;
        Id = Guid.CreateVersion7();
    }

    public Guid MovieId { get; private set; }
    public Guid HallId { get; }

    public Price TicketPrice { get; private set; }
    public virtual ShowTime ShowTime { get; private set; } = null!;

    /// <summary>
    ///     Updates the showtime for this schedule
    /// </summary>
    public void UpdateShowTime(ShowTime newShowTime)
    {
        ShowTime = Guard.Against.Null(newShowTime, nameof(newShowTime));
    }

    /// <summary>
    ///     Checks if the scheduled movie has started
    /// </summary>
    public bool HasStarted()
    {
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
        return ShowTime.HasStarted(currentTime);
    }

    /// <summary>
    ///     Checks if the scheduled movie has ended
    /// </summary>
    public bool HasEnded()
    {
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
        return ShowTime.HasEnded(currentTime);
    }

    /// <summary>
    ///     Gets schedule information as a formatted string
    /// </summary>
    public string GetDisplayInfo()
    {
        return $"Hall {HallId} - {ShowTime.GetDisplayInfo()}";
    }

    /// <summary>
    ///     Validates if the schedule can be created for the given date
    /// </summary>
    public bool CanScheduleForDate(DateOnly scheduleDate)
    {
        // Basic validation - can be extended with business rules
        return scheduleDate >= DateOnly.FromDateTime(DateTime.Today);
    }

    /// <summary>
    ///     Checks if this schedule can be cancelled (not started yet)
    /// </summary>
    public bool CanBeCancelled()
    {
        return !HasStarted();
    }
}