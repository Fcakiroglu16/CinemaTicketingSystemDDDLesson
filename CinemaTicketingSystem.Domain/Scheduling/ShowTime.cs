namespace CinemaTicketingSystem.Domain.Scheduling;

public class ShowTime : ValueObject
{
    private ShowTime()
    {
    }


    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public TimeSpan Duration => EndTime - StartTime;


    protected override IEnumerable<object?> GetEqualityComponents()
    {

        yield return StartTime;
        yield return EndTime;
    }
}