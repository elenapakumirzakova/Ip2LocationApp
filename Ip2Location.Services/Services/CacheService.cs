namespace Ip2Location.Business.Services;
public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public T? GetFromCache<T>(string key) where T : class
    {
        _cache.TryGetValue(key, out T? cachedResponse);
        return cachedResponse;
    }

    public T SetCache<T>(string key, T value) where T : class
    {
        var options = new MemoryCacheEntryOptions
        {
            // Cache for 1 hour
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromHours(1),
        };

        return _cache.Set(key, value, options);
    }

    public void ClearCache(string key)
    {
        _cache.Remove(key);
    }
}

