using CinemaTicketingSystem.Application.Abstraction.Catalog.Cinema.Hall;
using FluentValidation;

namespace CinemaTicketingSystem.API.Catalog.Cinema.Hall.Add;

public class AddCinemaHallValidator : AbstractValidator<AddCinemaHallRequest>
{
    public AddCinemaHallValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Technologies).NotEmpty();
        RuleFor(x => x.SeatList).NotEmpty().Must(x => x.Count > 0);
        RuleForEach(x => x.SeatList)
            .ChildRules(rules =>
            {
                rules.RuleFor(x => x.Row).NotEmpty().MaximumLength(10);
                rules.RuleFor(x => x.Number).GreaterThan(0);
                rules.RuleFor(x => x.seatType).IsInEnum();
            });
    }
}