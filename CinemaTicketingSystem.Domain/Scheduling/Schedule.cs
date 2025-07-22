namespace CinemaTicketingSystem.Domain.Scheduling;

public class Schedule : AggregateRoot<Guid>
{
    protected Schedule()
    {
    }

    public Schedule(Guid movieId, Guid hallId, ShowTime showTime)
    {
        if (movieId == Guid.Empty)
            throw new ArgumentException("Movie ID cannot be empty", nameof(movieId));

        if (hallId == Guid.Empty)
            throw new ArgumentException("Hall ID cannot be empty", nameof(hallId));

        if (showTime == null)
            throw new ArgumentNullException(nameof(showTime));


        MovieId = movieId;
        HallId = hallId;
        ShowTime = showTime;
        Id = Guid.CreateVersion7();
    }

    public Guid MovieId { get; private set; }
    public Guid HallId { get; private set; }
    public virtual ShowTime ShowTime { get; private set; }

    /// <summary>
    /// Updates the showtime for this schedule
    /// </summary>
    public void UpdateShowTime(ShowTime newShowTime)
    {
        if (newShowTime == null)
            throw new ArgumentNullException(nameof(newShowTime));

        var oldShowTime = ShowTime;
        ShowTime = newShowTime;

        // Domain event can be added here when needed
        // AddDomainEvent(new ScheduleShowTimeUpdatedEvent(Id, oldShowTime, newShowTime));
    }



    /// <summary>
    /// Checks if the scheduled movie has started
    /// </summary>
    public bool HasStarted()
    {
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
        return ShowTime.HasStarted(currentTime);
    }

    /// <summary>
    /// Checks if the scheduled movie has ended
    /// </summary>
    public bool HasEnded()
    {
        var currentTime = TimeOnly.FromDateTime(DateTime.Now);
        return ShowTime.HasEnded(currentTime);
    }








    /// <summary>
    /// Gets schedule information as a formatted string
    /// </summary>
    public string GetDisplayInfo()
    {
        return $"Hall {HallId} - {ShowTime.GetDisplayInfo()}";
    }

    /// <summary>
    /// Validates if the schedule can be created for the given date
    /// </summary>
    public bool CanScheduleForDate(DateOnly scheduleDate)
    {
        // Basic validation - can be extended with business rules
        return scheduleDate >= DateOnly.FromDateTime(DateTime.Today);
    }





    /// <summary>
    /// Checks if this schedule can be cancelled (not started yet)
    /// </summary>
    public bool CanBeCancelled()
    {
        return !HasStarted();
    }


}