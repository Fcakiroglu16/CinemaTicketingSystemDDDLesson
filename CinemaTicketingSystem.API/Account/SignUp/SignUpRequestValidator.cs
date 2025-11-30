#region

using CinemaTicketingSystem.Application.Contracts.Accounts;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Account.SignUp;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
    }
}