using CinemaTicketingSystem.Application.Abstraction.Contracts;
using CinemaTicketingSystem.Domain.Core;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.Json;

namespace CinemaTicketingSystem.API.Localization
{
    public class JsonStringLocalizerFactory(ILogger<JsonStringLocalizerFactory> logger, ICacheService cacheService) : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type? resourceSource)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


            var cacheKey = $"localization-{culture}";
            var cachedData = cacheService.Get<Dictionary<string, string>>(cacheKey);
            if (cachedData != null)
            {
                return new JsonStringLocalizer(cachedData);
            }

            var coreAssembly = typeof(CinemaConst).Assembly;


            var assemblyLocation = Path.GetDirectoryName(coreAssembly.Location); // örn: bin/Debug/net8.0/
            var filePath = Path.Combine(assemblyLocation!, "Resources", $"{culture}.json");


            if (!File.Exists(filePath))
            {
                logger.LogWarning("Localization file not found: {FilePath}", filePath);
                return new JsonStringLocalizer(new Dictionary<string, string>());
            }

            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);


            cacheService.Set(cacheKey, data);

            return new JsonStringLocalizer(data ?? []);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            var culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


            var coreAssembly = typeof(CinemaConst).Assembly;


            var assemblyLocation = Path.GetDirectoryName(coreAssembly.Location); // örn: bin/Debug/net8.0/
            var filePath = Path.Combine(assemblyLocation!, "Resources", $"{culture}.json");


            if (!File.Exists(filePath))
            {
                logger.LogWarning("Localization file not found: {FilePath}", filePath);
                return new JsonStringLocalizer(new Dictionary<string, string>());
            }

            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            return new JsonStringLocalizer(data ?? []);
        }
    }
}
