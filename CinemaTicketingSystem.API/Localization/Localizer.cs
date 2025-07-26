using CinemaTicketingSystem.Application.Abstraction.Contracts;
using Microsoft.Extensions.Localization;

namespace CinemaTicketingSystem.API.Localization;

public class Localizer(IStringLocalizer<SharedResource> stringLocalizer) : ILocalizer
{
    public string L(string key)
    {
        return stringLocalizer[key];
    }

    public string L(string key, params object[] data)
    {
        return string.Format(stringLocalizer[key], data);
    }
}