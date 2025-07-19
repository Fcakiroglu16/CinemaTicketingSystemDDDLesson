using CinemaTicketingSystem.Domain.CinemaManagement;
using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.Scheduling;

public class MovieSchedule : AggregateRoot<Guid>
{

    public Guid MovieId { get; set; }

    public Duration Duration { get; set; }

    public ScreeningTechnology SupportedTechnology { get; private set; } = ScreeningTechnology.Standard;

    private readonly List<ShowTime> showTimes = [];


    private List<CinemaHallSchedule> _cinemaHallSchedules = [];

    public virtual IReadOnlyList<CinemaHallSchedule> CinemaHallSchedules => _cinemaHallSchedules.AsReadOnly();

    protected MovieSchedule()
    {
    }

    public MovieSchedule(Guid movieId, Duration duration, ScreeningTechnology supportedTechnology)
    {
        Id = Guid.CreateVersion7();
        MovieId = movieId;
        Duration = duration;
        SupportedTechnology = supportedTechnology;
    }

    public virtual IReadOnlyCollection<ShowTime> ShowTimes => showTimes.AsReadOnly();

    public void AddShowTime(ShowTime showTime)
    {
        if (showTime == null)
            throw new ArgumentNullException(nameof(showTime));

        if (showTimes.Any(st => st.OverlapsWith(showTime)))
            throw new InvalidOperationException(
                $"Show time {showTime.GetTimeRange()} overlaps with existing show time");

        showTimes.Add(showTime);
    }

    public void AddShowTime(string timeRange)
    {
        var showTime = ShowTime.Create(timeRange);
        AddShowTime(showTime);
    }
}