using CinemaTicketingSystem.Application.Abstraction.Accounts;
using FluentValidation;

namespace CinemaTicketingSystem.API.Account.SignIn;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
    }
}