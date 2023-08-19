namespace Ip2Location.Business.Services.Interfaces;
public interface IIpStackHttpClient
{
    public Task<OneOf<TResponse, TErrorResponse>> SendAsync<TResponse, TErrorResponse>(HttpRequestMessage httpRequestMessage) where TResponse : class;
}

