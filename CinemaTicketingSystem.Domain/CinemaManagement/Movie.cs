

using CinemaTicketingSystem.Domain.CinemaManagement.DomainEvents;

namespace CinemaTicketingSystem.Domain.CinemaManagement
{
    public class Movie : AggregateRoot<Guid>
    {

        public string Title { get; private set; } = null!;
        public string? OriginalTitle { get; private set; }
        public string? Description { get; private set; }
        public Duration Duration { get; private set; } = null!;
        public DateTime? ShowingStartDate { get; private set; }
        public DateTime? ShowingEndDate { get; private set; }


        public bool IsCurrentlyShowing { get; private set; }
        private Movie() { }

        public Movie(string title, Duration duration)
        {
            Id = Guid.CreateVersion7();
            SetTitle(title);
            Duration = duration ?? throw new ArgumentNullException(nameof(duration));
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

            Title = title.Trim();
        }

        public void SetOriginalTitle(string? originalTitle)
        {
            OriginalTitle = string.IsNullOrWhiteSpace(originalTitle) ? null : originalTitle.Trim();
        }

        public string GetDisplayTitle()
        {
            return !string.IsNullOrEmpty(OriginalTitle) && OriginalTitle != Title
                ? $"{Title} ({OriginalTitle})"
                : Title;
        }
        public bool HasOriginalTitle() => !string.IsNullOrWhiteSpace(OriginalTitle);

        public void SetDescription(string? description)
        {
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        }

        public bool HasDescription() => !string.IsNullOrWhiteSpace(Description);

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

            ShowingStartDate = startDate ?? DateTime.UtcNow;

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

            ShowingStartDate = startDate.Date;
            ShowingEndDate = endDate?.Date;

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

    // Domain Events
}
