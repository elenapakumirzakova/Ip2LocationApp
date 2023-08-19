using OneOf;

namespace Ip2LocationApp.Integration.Interfaces;
public interface IIpStackHttpClient
{
    Task<OneOf<TResponse, TErrorResponse>> SendAsync<TResponse, TErrorResponse>(HttpRequestMessage httpRequestMessage) where TResponse : class;
}

