using CinemaTicketingSystem.Application.Abstraction.Schedule;
using FluentValidation;

namespace CinemaTicketingSystem.API.Schedule.AddMovieToHall;

public class AddMovieToHallValidator : AbstractValidator<AddMovieToHallRequest>
{

    public AddMovieToHallValidator()
    {
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.StartTime).NotEmpty();
        RuleFor(x => x.EndTime).NotEmpty();
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
    }
}