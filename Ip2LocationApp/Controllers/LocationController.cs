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
    [SwaggerResponse(400, "Bad Request")]
    [SwaggerResponse(422, "Validation Error")]
    public async Task<IActionResult> Get(string ip)
    {
        Console.WriteLine("Received new GET request with ip: {0}", ip);
        if (!ValidateIPAddress(ip))
        {
            return StatusCode(422, new ErrorResponseModel(Type, Error));
        }

        var ipLocation = CreateIpLocation(ip);
        await SaveIpLocation(ipLocation);

        var result = await _locationService.GetLocation(ipLocation);
        if (result.IsT0) 
            await UpdateIpLocation(result.AsT0);

        return result.Match<IActionResult>(
            success => Ok(MapSuccesResult(success)),
            error => StatusCode(500, MapError(error)));
    }

    private IpLocation CreateIpLocation(string ip)
    {
         return new IpLocation() { Ip = ip, CreateTime = DateTime.Now };
    }

    private async Task SaveIpLocation(IpLocation ipLocation)
    {
        try
        {
            await _repository.InsertAsync(ipLocation);
            await _repository.SaveAsync();
            Console.WriteLine("Successfully saved looked-up value in DB with ip: {0}, requestId: {1},", ipLocation.Ip, ipLocation.RequestId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to save looked-up value in DB with ip: {0}, requestId: {1},", ipLocation.Ip, ipLocation.RequestId);
        }
    }

    private async Task UpdateIpLocation(IpLocation ipLocation)
    {
        try
        {
            _repository.Update(ipLocation);
            await _repository.SaveAsync();
            Console.WriteLine("Successfully updated looked-up value in DB with ip: {0}, requestId: {1},", ipLocation.Ip, ipLocation.RequestId);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to update looked-up value in DB with ip: {0}, requestId: {1},", ipLocation.Ip, ipLocation.RequestId);
        }
    }

    private ResponseModel MapSuccesResult(IpLocation ipLocation)
    {
        Console.WriteLine("Map successful result ip: {0}", ipLocation.Ip);
        return new(
            ip: ipLocation.Ip,
            type: ipLocation.Type,
            country: ipLocation.Country,
            region: ipLocation.Region,
            city: ipLocation.City,
            zip: ipLocation.Zip,
            latitude: ipLocation.Latitude,
            longitude: ipLocation.Longitude);
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

