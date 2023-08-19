namespace Ip2Location.Business.Services.Interfaces;
public interface ILocationService
{
    Task<OneOf<Response, ErrorResponse>> GetLocation(string ip);
}
