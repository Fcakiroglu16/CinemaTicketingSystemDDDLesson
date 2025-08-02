using Ardalis.GuardClauses;
using CinemaTicketingSystem.Domain.BoundedContexts.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Catalog.DomainEvents;
using CinemaTicketingSystem.Domain.Core;
using CinemaTicketingSystem.SharedKernel.AggregateRoot;

namespace CinemaTicketingSystem.Domain.BoundedContexts.Catalog;

public class Movie : AggregateRoot<Guid>
{
    public Movie(string title, Duration duration, string posterImageUrl) : this(title, duration, posterImageUrl,
        ScreeningTechnology.Standard)
    {
    }

    public Movie(string title, Duration duration, string posterImageUrl, ScreeningTechnology supportedTechnology)
    {
        Id = Guid.CreateVersion7();
        SetTitle(title);
        SetPosterImageUrl(posterImageUrl);

        Duration = Guard.Against.Null(duration, nameof(duration));

        SupportedTechnology = supportedTechnology;

        AddDomainEvent(new MovieCreatedEvent(Id, duration, supportedTechnology));
    }

    public Movie()
    {
    }

    public string Title { get; private set; } = null!;
    public string? OriginalTitle { get; private set; }

    public ScreeningTechnology SupportedTechnology { get; private set; } = ScreeningTechnology.Standard;
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
        Guard.Against.InvalidInput(earliestDate,
            nameof(ShowingStartDate), x => ShowingStartDate.HasValue && ShowingStartDate < EarliestShowingDate,
            $"Current showing start date ({ShowingStartDate:yyyy-MM-dd}) cannot be earlier than earliest showing date ({EarliestShowingDate:yyyy-MM-dd})");

        EarliestShowingDate = earliestDate.Date;
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
        title = Guard.Against.NullOrWhiteSpace(title, nameof(title));
        Guard.Against.InvalidInput(title, nameof(title), x => title.Length > MovieConst.TitleMaxLength,
            $"Title cannot exceed {MovieConst.TitleMaxLength} characters");
        Title = title.Trim();
    }

    public void SetPosterImageUrl(string posterImageUrl)
    {
        posterImageUrl = Guard.Against.NullOrWhiteSpace(posterImageUrl, nameof(posterImageUrl));

        Guard.Against.InvalidInput(posterImageUrl, nameof(posterImageUrl),
            x => posterImageUrl.Length > MovieConst.PosterImageUrlMaxLength,
            $"Poster Image URL cannot exceed {MovieConst.PosterImageUrlMaxLength} characters");


        Guard.Against.InvalidInput(posterImageUrl, nameof(posterImageUrl),
            x => !Uri.TryCreate(posterImageUrl, UriKind.Absolute, out _),
            "Invalid poster image URL");

        PosterImageUrl = posterImageUrl.Trim();
    }

    public void SetOriginalTitle(string originalTitle)
    {
        originalTitle = Guard.Against.NullOrWhiteSpace(originalTitle, nameof(originalTitle));
        Guard.Against.InvalidInput(originalTitle, nameof(originalTitle),
            x => originalTitle.Length > MovieConst.OriginalTitleMaxLength,
            $"Original Title cannot exceed {MovieConst.OriginalTitleMaxLength} characters");

        OriginalTitle = originalTitle.Trim();
    }

    public bool HasOriginalTitle()
    {
        return !string.IsNullOrWhiteSpace(OriginalTitle);
    }

    public void SetDescription(string description)
    {
        description = Guard.Against.NullOrWhiteSpace(description, nameof(description));
        Guard.Against.InvalidInput(description, nameof(description),
            x => description.Length > MovieConst.DescriptionMaxLength,
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

        Guard.Against.NullOrWhiteSpace(Description, nameof(Description));
        return Description.Length <= maxLength
            ? Description
            : Description[..maxLength] + "...";
    }

    // Duration Helper Methods
    public void UpdateDuration(Duration newDuration)
    {
        Duration = Guard.Against.Null(newDuration, nameof(newDuration));
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


        Guard.Against.InvalidInput(proposedStartDate, nameof(startDate), x => !CanStartShowingOn(proposedStartDate),
            $"Movie cannot start showing on {proposedStartDate:yyyy-MM-dd}. Earliest allowed date is {EarliestShowingDate:yyyy-MM-dd}");

        ShowingStartDate = proposedStartDate;
        AddDomainEvent(new MovieShowingStartedEvent(Id, Title, ShowingStartDate.Value));
    }

    public void EndShowing(DateTime? endDate = null)
    {
        ShowingEndDate = endDate ?? DateTime.UtcNow;
        AddDomainEvent(new MovieShowingEndedEvent(Id, Title, ShowingEndDate.Value));
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