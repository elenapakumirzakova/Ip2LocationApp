namespace Ip2Location.Business.Services.Interfaces;
public interface ICacheService
{
    T GetFromCache<T>(string key) where T : class;
    T SetCache<T>(string key, T value) where T : class;
    void ClearCache(string key);
}

