#region

using CinemaTicketingSystem.Application.Contracts.Purchase;
using FluentValidation;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Purchase.Create;

internal class CreatePurchaseValidator : AbstractValidator<CreatePurchaseRequest>
{
    public CreatePurchaseValidator()
    {
        RuleFor(x => x.CardNumber).NotEmpty().CreditCard();
        RuleFor(x => x.CardHolderName).NotEmpty();
        RuleFor(x => x.CardExpirationDate).NotEmpty();
        RuleFor(x => x.CardSecurityNumber).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.TicketIssuanceId).NotEmpty();
    }
}