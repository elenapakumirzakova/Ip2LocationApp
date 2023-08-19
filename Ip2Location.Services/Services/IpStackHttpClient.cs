namespace Ip2Location.Business.Services;
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
            Console.WriteLine("About to send http request to IpStack Uri", httpRequestMessage.RequestUri);
            using var responseMessage = await _httpClient.SendAsync(httpRequestMessage);
            string responseBody = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                Console.WriteLine("Received successful response from IpStack with payload: {0}", await responseMessage.Content.ReadAsStringAsync());
                return MapSuccessResponse<TResponse>(responseBody);
            }

            Console.WriteLine("Received failed response from IpStack with error: {0}", await responseMessage.Content.ReadAsStringAsync());
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