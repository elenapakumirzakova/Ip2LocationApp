using Ip2LocationApp.Integration.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneOf;

namespace Ip2LocationApp.Integration;
public class IpStackHttpClient : IIpStackHttpClient
{
    private readonly HttpClient _httpClient;

    public IpStackHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<OneOf<TResponse, TErrorResponse>> SendAsync<TResponse, TErrorResponse>(HttpRequestMessage httpRequestMessage) where TResponse : class
    {
        try
        {
            using var responseMessage = await _httpClient.SendAsync(httpRequestMessage);
            string responseBody = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                return MapSuccessResponse<TResponse>(responseBody);
            }

            return MapErrorResponse<TErrorResponse>(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);
            return null;
        }
    }

    private static TResponse MapSuccessResponse<TResponse>(string responseBody) where TResponse : class
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            }
        };

        return JsonConvert.DeserializeObject<TResponse>(responseBody, settings);
    }

    private static TErrorResponse MapErrorResponse<TErrorResponse>(string responseBody)
    {
        return JsonConvert.DeserializeObject<TErrorResponse>(responseBody);
    }
}