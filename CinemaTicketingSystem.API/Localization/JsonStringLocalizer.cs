using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CinemaTicketingSystem.API.Localization
{
    public class JsonStringLocalizer(IDictionary<string, string> localizedStrings) : IStringLocalizer
    {
        public LocalizedString this[string name]
            => new(name, localizedStrings.ContainsKey(name) ? localizedStrings[name] : name, !localizedStrings.ContainsKey(name));

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = localizedStrings.ContainsKey(name) ? localizedStrings[name] : name;
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, !localizedStrings.ContainsKey(name));
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return localizedStrings.Select(pair => new LocalizedString(pair.Key, pair.Value, false));
        }

        public IStringLocalizer WithCulture(CultureInfo culture) => this;

    }

}
