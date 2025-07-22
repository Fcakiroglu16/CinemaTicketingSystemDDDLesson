namespace CinemaTicketingSystem.Domain;

public class Duration : ValueObject
{
    public Duration(int minutes)
    {
        if (minutes <= 0)
            throw new ArgumentException("Duration must be positive", nameof(minutes));
        if (minutes > 600) // 10 saat maksimum
            throw new ArgumentException("Duration cannot exceed 600 minutes", nameof(minutes));

        Minutes = minutes;
    }

    public int Minutes { get; }
    public int Hours => Minutes / 60;
    public int RemainingMinutes => Minutes % 60;

    public TimeSpan ToTimeSpan()
    {
        return TimeSpan.FromMinutes(Minutes);
    }

    public string GetFormattedDuration()
    {
        if (Hours == 0)
            return $"{Minutes}m";

        return RemainingMinutes == 0
            ? $"{Hours}h"
            : $"{Hours}h {RemainingMinutes}m";
    }

    public bool IsShortMovie()
    {
        return Minutes < 90;
    }

    public bool IsFeatureLength()
    {
        return Minutes >= 90;
    }

    public bool IsLongMovie()
    {
        return Minutes > 180;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Minutes;
    }

    public static Duration FromHours(int hours)
    {
        return new Duration(hours * 60);
    }

    public static Duration FromHoursAndMinutes(int hours, int minutes)
    {
        return new Duration(hours * 60 + minutes);
    }

    public override string ToString()
    {
        return GetFormattedDuration();
    }
}