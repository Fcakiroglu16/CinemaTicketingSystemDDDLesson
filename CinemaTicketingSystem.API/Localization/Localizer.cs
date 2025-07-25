using CinemaTicketingSystem.Application.Abstraction.Contracts;
using Microsoft.Extensions.Localization;

namespace CinemaTicketingSystem.API.Localization
{
    internal class Localizer(IStringLocalizer<SharedResource> stringLocalizer) : ILocalizer
    {
        public string L(string key)
        {
            return stringLocalizer[key.ToLower()] ?? key;
        }
    }
}
