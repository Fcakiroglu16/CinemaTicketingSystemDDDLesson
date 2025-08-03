using CinemaTicketingSystem.Application.Abstraction.Contracts;
using Microsoft.Extensions.Localization;

namespace CinemaTicketingSystem.API.Localization;

public class Localizer(IStringLocalizer<SharedResource> stringLocalizer) : ILocalizer
{


    public string L(string key, object[]? data)
    {
        return data?.Length > 0 ? string.Format(stringLocalizer[key], data) : stringLocalizer[key];
    }
}