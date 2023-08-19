namespace Ip2LocationApp.Controllers;

[Route("[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private const string Type = "validation_error";
    private const string Error = "IP address is invalid.";

    private readonly ILocationService _locationService;
    private readonly IRepository<IpLocation> _repository;

    public LocationController(ILocationService locationService, IRepository<IpLocation> repository)
    {
        _locationService = locationService;
        _repository = repository;
    }

    [HttpGet("")]
    [SwaggerOperation(Summary = "Get Location by Ip", Description = "The primary endpoint to look up single IPv4 or IPv6 addresses. To call this endpoint, simply attach any IPv4 or IPv6 address to the API's base URL.")]
    [SwaggerResponse(200, "Success")]
    [SwaggerResponse(422, "Validation Error")]
    public async Task<IActionResult> Get(string ip)
    {
        Console.WriteLine("Received new GET request with ip: {0}", ip);
        if (!ValidateIPAddress(ip))
        {
            return StatusCode(422, new ErrorResponseModel(Type, Error));
        }

        await SaveRequest(ip);
        var result = await _locationService.GetLocation(ip);

        return result.Match<IActionResult>(
            success => Ok(MapSuccesResult(success)),
            error => StatusCode(500, MapError(error)));
    }

    private async Task SaveRequest(string ip)
    {
        try
        {
            var entity = new IpLocation() { Ip = ip, CreateTime = DateTime.Now };
            await _repository.InsertAsync(entity);
            await _repository.SaveAsync();
            Console.WriteLine("Successfully to saved request to DB: {0}", entity.RequestId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save request to DB: {0}", ex.Message);
        }
    }

    private ResponseModel MapSuccesResult(Response success)
    {
        Console.WriteLine("Map successful result ip: {0}", success.Ip);
        return new(
            ip: success.Ip,
            type: success.Type,
            country: success.CountryName,
            region: success.RegionName,
            city: success.City,
            zip: success.Zip,
            latitude: success.Latitude,
            longitude: success.Longitude);
    }

    private ErrorResponseModel MapError(ErrorResponse error)
    {
        return new(error.Error?.Type, error.Error?.Info);
    }

    private static bool ValidateIPAddress(string ipAddress)
    {
        IPAddress parsedIPAddress;
        return IPAddress.TryParse(ipAddress, out parsedIPAddress);
    }
}

