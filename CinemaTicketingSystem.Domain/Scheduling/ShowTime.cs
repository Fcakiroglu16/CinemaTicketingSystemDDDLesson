namespace CinemaTicketingSystem.Domain.Scheduling;

public class ShowTime : Entity<Guid>
{
    public TimeSpan StartTime { get; private set; }
    public TimeSpan EndTime { get; private set; }
    public TimeSpan Duration => EndTime - StartTime;
    public virtual MovieSchedule MovieSchedule { get; set; } = null!;
    private ShowTime() { }

    public ShowTime(TimeSpan startTime, TimeSpan endTime)
    {
        if (startTime >= endTime)
            throw new ArgumentException("Start time must be before end time");

        if (startTime < TimeSpan.Zero || startTime >= TimeSpan.FromDays(1))
            throw new ArgumentException("Start time must be within a 24-hour period");

        if (endTime < TimeSpan.Zero || endTime >= TimeSpan.FromDays(1))
            throw new ArgumentException("End time must be within a 24-hour period");

        StartTime = startTime;
        EndTime = endTime;
    }

    // Factory methods
    public static ShowTime Create(int startHour, int startMinute, int endHour, int endMinute)
    {
        var startTime = new TimeSpan(startHour, startMinute, 0);
        var endTime = new TimeSpan(endHour, endMinute, 0);
        return new ShowTime(startTime, endTime);
    }

    public static ShowTime Create(string timeRange)
    {
        // "17:00-19:00" formatını parse eder
        var parts = timeRange.Split('-');
        if (parts.Length != 2)
            throw new ArgumentException("Invalid time range format. Expected format: HH:mm-HH:mm");

        if (!TimeSpan.TryParse(parts[0].Trim(), out var startTime))
            throw new ArgumentException("Invalid start time format");

        if (!TimeSpan.TryParse(parts[1].Trim(), out var endTime))
            throw new ArgumentException("Invalid end time format");

        return new ShowTime(startTime, endTime);
    }

    // Business methods
    public bool OverlapsWith(ShowTime other)
    {
        if (other == null) return false;

        return StartTime < other.EndTime && EndTime > other.StartTime;
    }

    public bool Contains(TimeSpan time)
    {
        return time >= StartTime && time <= EndTime;
    }

    public string GetTimeRange()
    {
        return $"{StartTime:hh\\:mm}-{EndTime:hh\\:mm}";
    }

    public string GetTimeRangeWithDuration()
    {
        return $"{GetTimeRange()} ({Duration.TotalMinutes} dakika)";
    }

    public bool IsValidForToday()
    {
        var now = DateTime.Now.TimeOfDay;
        return StartTime > now;
    }

    // Güncelleme metodları
    public void UpdateTimes(TimeSpan newStartTime, TimeSpan newEndTime)
    {
        if (newStartTime >= newEndTime)
            throw new ArgumentException("Start time must be before end time");

        StartTime = newStartTime;
        EndTime = newEndTime;
    }

    public override string ToString()
    {
        return GetTimeRange();
    }



    //    // Farklı oluşturma yöntemleri
    //    var showTime1 = new ShowTime(new TimeSpan(17, 0, 0), new TimeSpan(19, 0, 0));
    //    var showTime2 = ShowTime.Create(17, 0, 19, 0);
    //    var showTime3 = ShowTime.Create("17:00-19:00");

    //    // Business metodları
    //    Console.WriteLine(showTime1.GetTimeRange()); // "17:00-19:00"
    //    Console.WriteLine(showTime1.Duration.TotalMinutes); // 120

    //// Çakışma kontrolü
    //    var evening = ShowTime.Create("18:30-20:30");
    //    bool overlaps = showTime1.OverlapsWith(evening); // true

    //    // Zaman içinde olma kontrolü
    //    bool contains = showTime1.Contains(new TimeSpan(18, 0, 0)); // true
}