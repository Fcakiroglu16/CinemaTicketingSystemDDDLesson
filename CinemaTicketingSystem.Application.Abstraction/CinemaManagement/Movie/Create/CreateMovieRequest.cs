namespace CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create
{
    public record CreateMovieRequest(
        string Title,
        string? OriginalTitle,
        string? Description,
        TimeSpan Duration,
        DateTime? EarliestShowingDate);

}
