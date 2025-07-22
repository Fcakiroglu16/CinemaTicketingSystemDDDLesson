using CinemaTicketingSystem.Domain.Core;

namespace CinemaTicketingSystem.Application.Abstraction.Catalog.Movie;

public record MovieDto(
    Guid Id,
    string Title,
    string? OriginalTitle,
    string PosterImageUrl,
    string? Description,
    double DurationMinutes,
    string DurationFormatted,
    ScreeningTechnology SupportedTechnology
);