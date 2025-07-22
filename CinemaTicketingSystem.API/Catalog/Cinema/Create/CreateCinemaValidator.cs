using CinemaTicketingSystem.Application.Abstraction.CinemaManagement.Cinema;
using CinemaTicketingSystem.Domain.Core;
using FluentValidation;

namespace CinemaTicketingSystem.API.Catalog.Cinema.Create;

public class CreateCinemaValidator : AbstractValidator<CreateCinemaRequest>
{
    public CreateCinemaValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(CinemaConst.NameMaxLength);

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.Address.Country)
            .NotEmpty()
            .MaximumLength(CinemaConst.CountryMaxLength);

        RuleFor(x => x.Address.City)
            .NotEmpty()
            .MaximumLength(CinemaConst.CityMaxLength);

        RuleFor(x => x.Address.District)
            .NotEmpty()
            .MaximumLength(CinemaConst.DistrictMaxLength);

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .MaximumLength(CinemaConst.StreetMaxLength);

        RuleFor(x => x.Address.PostalCode)
            .NotEmpty()
            .MaximumLength(CinemaConst.PostalCodeMaxLength);

        RuleFor(x => x.Address.Description)
            .MaximumLength(CinemaConst.AddressDescriptionMaxLength);
    }
}