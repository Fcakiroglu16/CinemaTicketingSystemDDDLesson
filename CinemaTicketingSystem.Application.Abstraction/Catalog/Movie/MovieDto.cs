#region

#endregion

using CinemaTicketingSystem.SharedKernel;

namespace CinemaTicketingSystem.Application.Contracts.Catalog.Movie;

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