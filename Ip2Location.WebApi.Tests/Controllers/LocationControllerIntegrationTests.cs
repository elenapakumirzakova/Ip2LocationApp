using System.Net.Http.Json;

namespace Ip2Location.WebApi.Tests.Controllers;
public class LocationControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public LocationControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("95.90.240.199")]
    [InlineData("2a02:8109:8a00:238:19ff:ef9f:f445:feb9")]
    public async Task Get_EndpointReturnSuccess(string ip)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(string.Format("api/location?ip={0}", ip));

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseModel = await response.Content.ReadFromJsonAsync<ResponseModel>();
        Assert.Equal("Germany", responseModel.Country);
        Assert.Equal(ip, responseModel.Ip);
    }

    [Fact]
    public async Task Get_EndpointReturnValidationError()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(string.Format("api/location?ip={0}", "abc"));

        // Assert
        Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponseModel>();
        Assert.Equal("validation_error", errorResponse.Type);
        Assert.Equal("IP address is invalid.", errorResponse.Info);
    }
}