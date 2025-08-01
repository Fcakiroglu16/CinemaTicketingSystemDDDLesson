using CinemaTicketingSystem.Application.Abstraction.Ticketing;
using FluentValidation;

namespace CinemaTicketingSystem.API.Ticketing.Reservation.Reserve;

public class ReserveSeatsRequestValidator : AbstractValidator<ReserveSeatsRequest>
{
    public ReserveSeatsRequestValidator()
    {
        RuleFor(x => x.SeatPositionList).NotEmpty();
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
        RuleFor(x => x.SeatPositionList).Must(x => x.Count > 0).WithMessage("Please select at least one seat.");
    }
}