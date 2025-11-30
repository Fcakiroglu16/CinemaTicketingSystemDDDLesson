namespace CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;

public record CreateMovieRequest(
    string Title,
    string PosterImageUrl,
    string? OriginalTitle,
    string? Description,
    TimeSpan Duration,
    DateTime? EarliestShowingDate);