namespace Ip2Location.Business.Services;
public class LocationService : ILocationService
{
    private const string Path = "{0}?access_key={1}";

    private readonly IIpStackHttpClient _ipStackHttpClient;
    private readonly IpStackOptions _options;
    private readonly ICacheService _cacheService;

    public LocationService(IIpStackHttpClient ipStackHttpClient, IOptions<IpStackOptions> options, ICacheService cacheService)
    {
        _ipStackHttpClient = ipStackHttpClient;
        _options = options.Value;
        _cacheService = cacheService;
    }

    public async Task<OneOf<Response, ErrorResponse>> GetLocation(string ip)
    {
        var cachedData = _cacheService.GetFromCache<Response>(ip);
        if (cachedData is not null)
        {
            Console.WriteLine("Retrieved data from cache for ip: {0}", cachedData.Ip);
            return cachedData;
        }

        var response = await SendPartnerRequest(ip);
        SetCache(ip, response);

        return response;
    }

    private async Task<OneOf<Response, ErrorResponse>> SendPartnerRequest(string ip)
    {
        var path = string.Format(Path, ip, _options.AccessKey);
        var message = new HttpRequestMessage(HttpMethod.Get, path);
        var response = await _ipStackHttpClient.SendAsync<Response, ErrorResponse>(message);
        return response;
    }

    private void SetCache(string ip, OneOf<Response, ErrorResponse> response)
    {
        Console.WriteLine("Set cache for ip: {0}", ip);
        response.Match<OneOf<Response, ErrorResponse>>(
            success => _cacheService.SetCache<Response>(ip, success),
            error => _cacheService.SetCache<ErrorResponse>(ip, error));
    }
}
