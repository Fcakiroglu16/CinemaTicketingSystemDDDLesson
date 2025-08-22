#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.SeatHold.Create;

public class CreateSeatHoldRequestValidator : AbstractValidator<CreateSeatHoldRequest>
{
    public CreateSeatHoldRequestValidator()
    {
        RuleFor(x => x.SeatPositions).NotEmpty();
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
    }
}