namespace Ip2Location.Business.Services;
public class LocationService : ILocationService
{
    private const string Path = "{0}?access_key={1}";

    private readonly IIpStackHttpClient _ipStackHttpClient;
    private readonly IpStackOptions _options;
    private readonly ICacheService _cacheService;

    public LocationService(
        IIpStackHttpClient ipStackHttpClient,
        IOptions<IpStackOptions> options,
        ICacheService cacheService)
    {
        _ipStackHttpClient = ipStackHttpClient;
        _options = options.Value;
        _cacheService = cacheService;
    }

    public async Task<OneOf<IpLocation, ErrorResponse>> GetLocation(IpLocation ipLocation)
    {
        var cachedData = _cacheService.GetFromCache<IpLocation>(ipLocation.Ip);
        if (cachedData is not null)
        {
            Console.WriteLine("Retrieved data from cache for ip: {0}, requestId: {1}.", cachedData.Ip, cachedData.RequestId);
            return cachedData;
        }

        var response = await SendPartnerRequest(ipLocation.Ip, ipLocation.RequestId);

        return response.Match<OneOf<IpLocation, ErrorResponse>>(
            success => SetCache(ipLocation.Ip, MapResponse(ipLocation, success)),
            error => error);
    }

    private IpLocation MapResponse(IpLocation ipLocation, Response response)
    {
        ipLocation.Type = response.Type;
        ipLocation.Country = response.CountryName;
        ipLocation.Region = response.RegionName;
        ipLocation.City = response.City;
        ipLocation.Zip = response.Zip;
        ipLocation.Latitude = response.Latitude;
        ipLocation.Longitude = response.Longitude;

        return ipLocation;
    }

    private async Task<OneOf<Response, ErrorResponse>> SendPartnerRequest(string ip, string requestId)
    {
        var path = string.Format(Path, ip, _options.AccessKey);
        var message = new HttpRequestMessage(HttpMethod.Get, path);
        var response = await _ipStackHttpClient.SendAsync<Response, ErrorResponse>(message, requestId);
        return response;
    }

    private IpLocation SetCache(string ip, IpLocation ipLocation)
    {
        Console.WriteLine("Set cache for ip: {0}", ip);
        return _cacheService.SetCache<IpLocation>(ip, ipLocation);
    }
}
