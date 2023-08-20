namespace Ip2Location.Business.Services.Interfaces;
public interface ILocationService
{
    Task<OneOf<IpLocation, ErrorResponse>> GetLocation(IpLocation ipLocation);
}
