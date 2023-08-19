namespace Ip2Location.Business.Models;
public class CacheOptions
{
    public const string Key = "CacheOptions";
    public int ExpirationTime { get; set; }
    public int SlidingExpiration { get; set; }
}

