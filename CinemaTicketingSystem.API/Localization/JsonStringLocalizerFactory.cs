#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using CinemaTicketingSystem.SharedKernel;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Reflection;
using System.Text.Json;

#endregion

namespace CinemaTicketingSystem.Presentation.API.Localization;

public class JsonStringLocalizerFactory(ILogger<JsonStringLocalizerFactory> logger, ICacheService cacheService)
    : IStringLocalizerFactory
{
    public IStringLocalizer Create(Type? resourceSource)
    {
        string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


        string cacheKey = $"localization-{culture}";
        Dictionary<string, string>? cachedData = cacheService.Get<Dictionary<string, string>>(cacheKey);
        if (cachedData != null) return new JsonStringLocalizer(cachedData);

        Assembly coreAssembly = typeof(CinemaConst).Assembly;


        string? assemblyLocation = Path.GetDirectoryName(coreAssembly.Location); // örn: bin/Debug/net8.0/
        string filePath = Path.Combine(assemblyLocation!, "Resources", $"{culture}.json");


        if (!File.Exists(filePath))
        {
            logger.LogWarning("Localization file not found: {FilePath}", filePath);
            return new JsonStringLocalizer(new Dictionary<string, string>());
        }

        string json = File.ReadAllText(filePath);
        Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);


        cacheService.Set(cacheKey, data);

        return new JsonStringLocalizer(data ?? []);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        string culture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;


        Assembly coreAssembly = typeof(CinemaConst).Assembly;


        string? assemblyLocation = Path.GetDirectoryName(coreAssembly.Location); // örn: bin/Debug/net8.0/
        string filePath = Path.Combine(assemblyLocation!, "Resources", $"{culture}.json");


        if (!File.Exists(filePath))
        {
            logger.LogWarning("Localization file not found: {FilePath}", filePath);
            return new JsonStringLocalizer(new Dictionary<string, string>());
        }

        string json = File.ReadAllText(filePath);
        Dictionary<string, string>? data = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        return new JsonStringLocalizer(data ?? []);
    }
}