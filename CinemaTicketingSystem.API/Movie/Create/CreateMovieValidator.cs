using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Movie.Create;
using CinemaTicketingSystem.Domain.Core;
using FluentValidation;

namespace CinemaTicketingSystem.API.Movie.Create
{
    internal class CreateMovieValidator : AbstractValidator<CreateMovieRequest>
    {

        public CreateMovieValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(MovieConst.TitleMaxLength);
            RuleFor(x => x.OriginalTitle).MaximumLength(MovieConst.OriginalTitleMaxLength);
            RuleFor(x => x.Description).MaximumLength(MovieConst.DescriptionMaxLength);
            RuleFor(x => x.Duration).GreaterThan(TimeSpan.Zero);
            RuleFor(x => x.EarliestShowingDate).GreaterThanOrEqualTo(DateTime.UtcNow);
        }
    }
}
