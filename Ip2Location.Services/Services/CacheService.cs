namespace Ip2Location.Business.Services;
public class CacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly CacheOptions _options;

    public CacheService(IMemoryCache cache, IOptions<CacheOptions> options)
    {
        _cache = cache;
        _options = options.Value;
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
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.ExpirationTime),
            SlidingExpiration = TimeSpan.FromHours(_options.SlidingExpiration),
        };

        return _cache.Set(key, value, options);
    }

    public void ClearCache(string key)
    {
        _cache.Remove(key);
    }
}

