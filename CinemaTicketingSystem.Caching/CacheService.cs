#region

using CinemaTicketingSystem.Application.Contracts.Contracts;
using Microsoft.Extensions.Caching.Memory;

#endregion

namespace CinemaTicketingSystem.Infrastructure.Caching;

public class CacheService(IMemoryCache memoryCache) : ICacheService
{
    public T? Get<T>(string key)
    {
        return memoryCache.TryGetValue(key, out object? value) ? (T?)value : default;
    }

    public void Set<T>(string key, T value, TimeSpan? expiration = null)
    {
        MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();

        if (expiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiration;
        else
            // Default expiration of 1 hour if not specified
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

        memoryCache.Set(key, value, options);
    }

    public void Remove(string key)
    {
        memoryCache.Remove(key);
    }

    public bool Exists(string key)
    {
        return memoryCache.TryGetValue(key, out _);
    }
}