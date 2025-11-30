#region

using CinemaTicketingSystem.Application.Contracts.Catalog.Movie.Create;
using CinemaTicketingSystem.SharedKernel;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Catalog.Movie.Create;

public class CreateMovieValidator : AbstractValidator<CreateMovieRequest>
{
    public CreateMovieValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(MovieConst.TitleMaxLength);
        RuleFor(x => x.OriginalTitle).MaximumLength(MovieConst.OriginalTitleMaxLength);
        RuleFor(x => x.Description).MaximumLength(MovieConst.DescriptionMaxLength);
        RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
        RuleFor(x => x.EarliestShowingDate).GreaterThanOrEqualTo(DateTime.UtcNow);


        RuleFor(x => x.PosterImageUrl).NotEmpty()
            .Must(BeAValidUrl)
            .WithMessage("Poster image URL must be a valid URL.");
    }

    private bool BeAValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}