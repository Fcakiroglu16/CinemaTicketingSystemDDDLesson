#region

using CinemaTicketingSystem.Application.Contracts.Ticketing;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Ticketing.TicketIssuance.Create;

public class CreateTicketIssuanceRequestValidator : AbstractValidator<CreateTicketIssuanceRequest>
{
    public CreateTicketIssuanceRequestValidator()
    {
        RuleFor(x => x.ScheduledMovieShowId).NotEmpty();
    }
}