using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;
using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Domain.CinemaManagement;

public class Movie : AggregateRoot<Guid>
{


    public Movie(string title, Duration duration, string posterImageUrl)
    {
        Id = Guid.CreateVersion7();
        SetTitle(title);
        SetPosterImageUrl(posterImageUrl);

        Duration = duration ?? throw new ArgumentNullException(nameof(duration));

        AddDomainEvent(new MovieCreatedEvent(Id));
    }

    public Movie() { }
    public string Title { get; private set; } = null!;
    public string? OriginalTitle { get; private set; }

    public string PosterImageUrl { get; private set; } = null!;
    public string? Description { get; private set; }
    public Duration Duration { get; private set; } = null!;
    public DateTime? EarliestShowingDate { get; private set; }
    public DateTime? ShowingStartDate { get; private set; }
    public DateTime? ShowingEndDate { get; private set; }
    public bool IsCurrentlyShowing { get; private set; }

    // Earliest Showing Date Management
    public void SetEarliestShowingDate(DateTime earliestDate)
    {
        EarliestShowingDate = earliestDate.Date;

        if (ShowingStartDate.HasValue && ShowingStartDate < EarliestShowingDate)
            throw new InvalidOperationException(
                $"Current showing start date ({ShowingStartDate:yyyy-MM-dd}) cannot be earlier than earliest showing date ({EarliestShowingDate:yyyy-MM-dd})");
    }

    public void ClearEarliestShowingDate()
    {
        EarliestShowingDate = null;
    }

    public bool CanStartShowingOn(DateTime date)
    {
        if (!EarliestShowingDate.HasValue)
            return true;

        return date.Date >= EarliestShowingDate.Value;
    }

    public int GetDaysUntilEarliestShowing()
    {
        if (!EarliestShowingDate.HasValue)
            return 0;

        var days = (EarliestShowingDate.Value - DateTime.Today).Days;
        return Math.Max(0, days);
    }

    public bool IsAvailableForImmediateShowing()
    {
        return !EarliestShowingDate.HasValue || DateTime.Today >= EarliestShowingDate.Value;
    }

    public void SetCurrentlyShowing(bool isShowing)
    {
        IsCurrentlyShowing = isShowing;
        if (isShowing)
        {
            if (!ShowingStartDate.HasValue)
                StartShowing();
        }
        else
        {
            if (ShowingStartDate.HasValue) EndShowing();
        }
    }

    // Title Management Helper Methods
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));


        if (title.Length > MovieConst.TitleMaxLength)
            throw new ArgumentOutOfRangeException(nameof(title),
                $"Title cannot exceed {MovieConst.TitleMaxLength} characters");
        Title = title.Trim();
    }


    public void SetPosterImageUrl(string posterImageUrl)
    {
        if (string.IsNullOrWhiteSpace(posterImageUrl))
            throw new ArgumentException("Poster Image URL cannot be empty", nameof(posterImageUrl));
        if (posterImageUrl.Length > MovieConst.PosterImageUrlMaxLength)
            throw new ArgumentOutOfRangeException(nameof(posterImageUrl),
                $"Poster Image URL cannot exceed {MovieConst.PosterImageUrlMaxLength} characters");


        if (!Uri.TryCreate(posterImageUrl, UriKind.Absolute, out _))
            throw new ArgumentException("Invalid poster image URL", nameof(posterImageUrl));


        PosterImageUrl = posterImageUrl.Trim();
    }
    public void SetOriginalTitle(string originalTitle)
    {
        if (string.IsNullOrWhiteSpace(originalTitle))
            throw new ArgumentException("Original Title  cannot be empty", nameof(originalTitle));


        if (originalTitle.Length > MovieConst.OriginalTitleMaxLength)
            throw new ArgumentOutOfRangeException(nameof(OriginalTitle),
                $"Original Title cannot exceed {MovieConst.OriginalTitleMaxLength} characters");

        OriginalTitle = originalTitle.Trim();
    }


    public bool HasOriginalTitle()
    {
        return !string.IsNullOrWhiteSpace(OriginalTitle);
    }

    public void SetDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty", nameof(description));


        if (description.Length > MovieConst.DescriptionMaxLength)
            throw new ArgumentOutOfRangeException(nameof(Description),
                $"Description cannot exceed {MovieConst.DescriptionMaxLength} characters");


        Description = description.Trim();
    }

    public bool HasDescription()
    {
        return !string.IsNullOrWhiteSpace(Description);
    }

    public string GetShortDescription(int maxLength = 100)
    {
        if (string.IsNullOrWhiteSpace(Description))
            return "No description available";

        return Description.Length <= maxLength
            ? Description
            : Description[..maxLength] + "...";
    }

    // Duration Helper Methods
    public void UpdateDuration(Duration newDuration)
    {
        Duration = newDuration ?? throw new ArgumentNullException(nameof(newDuration));
    }

    public void UpdateDuration(int minutes)
    {
        Duration = new Duration(minutes);
    }

    public void UpdateDuration(int hours, int minutes)
    {
        Duration = Duration.FromHoursAndMinutes(hours, minutes);
    }

    public string GetDurationInfo()
    {
        var info = Duration.GetFormattedDuration();

        if (Duration.IsShortMovie())
            info += " (Short Film)";
        else if (Duration.IsLongMovie())
            info += " (Extended Length)";

        return info;
    }

    // Showing Status Management
    public void StartShowing(DateTime? startDate = null)
    {
        var proposedStartDate = startDate ?? DateTime.UtcNow;

        // En erken gösterim tarihini kontrol et
        if (!CanStartShowingOn(proposedStartDate))
            throw new InvalidOperationException(
                $"Movie cannot start showing on {proposedStartDate:yyyy-MM-dd}. Earliest allowed date is {EarliestShowingDate:yyyy-MM-dd}");

        ShowingStartDate = proposedStartDate;
        AddDomainEvent(new MovieShowingStartedEvent(Id, Title, ShowingStartDate.Value));
    }

    public void EndShowing(DateTime? endDate = null)
    {
        ShowingEndDate = endDate ?? DateTime.UtcNow;
        AddDomainEvent(new MovieShowingEndedEvent(Id, Title, ShowingEndDate.Value));
    }

    public void SetShowingPeriod(DateTime startDate, DateTime? endDate = null)
    {
        if (startDate > DateTime.Today.AddYears(1))
            throw new ArgumentException("Start date cannot be more than 1 year in the future", nameof(startDate));

        if (endDate.HasValue && endDate <= startDate)
            throw new ArgumentException("End date must be after start date", nameof(endDate));

        // En erken gösterim tarihini kontrol et
        if (!CanStartShowingOn(startDate))
            throw new InvalidOperationException(
                $"Showing cannot start on {startDate:yyyy-MM-dd}. Earliest allowed date is {EarliestShowingDate:yyyy-MM-dd}");

        ShowingStartDate = startDate.Date;
        ShowingEndDate = endDate?.Date;
    }

    // Query Methods
    public string GetShowingAvailabilityStatus()
    {
        if (!EarliestShowingDate.HasValue)
            return "Available for immediate showing";

        if (DateTime.Today >= EarliestShowingDate.Value)
            return "Available for showing";

        var daysRemaining = GetDaysUntilEarliestShowing();
        return $"Available for showing in {daysRemaining} days ({EarliestShowingDate:MMM dd, yyyy})";
    }

    public bool IsShowingOn(DateTime date)
    {
        if (!ShowingStartDate.HasValue)
            return false;

        var checkDate = date.Date;
        return checkDate >= ShowingStartDate.Value &&
               (!ShowingEndDate.HasValue || checkDate <= ShowingEndDate.Value);
    }

    public bool WillBeShowingInPeriod(DateTime startDate, DateTime endDate)
    {
        if (!ShowingStartDate.HasValue)
            return false;

        var movieStart = ShowingStartDate.Value;
        var movieEnd = ShowingEndDate ?? DateTime.MaxValue.Date;

        return movieStart <= endDate.Date && movieEnd >= startDate.Date;
    }
}