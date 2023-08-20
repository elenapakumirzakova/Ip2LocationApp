namespace Ip2Location.Business.Tests.Services;
public class LocationServiceTests
{
    [Fact]
    public void Get_Location_HappyPath()
    {
        // Arrange
        var fixture = new Fixture();
        var cacheService = new Mock<ICacheService>();
        var ipStackHttpClient = new Mock<IIpStackHttpClient>();
        var ipLocation = fixture.Create<IpLocation>();
        var response = OneOf<Response, ErrorResponse>.FromT0(fixture.Create<Response>());
        var options = Options.Create(fixture.Create<IpStackOptions>());

        cacheService.Setup(cs => cs.GetFromCache<Response>(It.IsAny<string>())).Returns((Response)null);
        ipStackHttpClient.Setup(hc => hc.SendAsync<Response, ErrorResponse>(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).ReturnsAsync(response);

        var service = new LocationService(ipStackHttpClient.Object, options, cacheService.Object);

        // Act
        var result = service.GetLocation(ipLocation);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Get_Location_With_ErrorResult()
    {
        // Arrange
        var fixture = new Fixture();
        var ipLocation = fixture.Create<IpLocation>();
        var errorResponse = OneOf<Response, ErrorResponse>.FromT1(fixture.Create<ErrorResponse>());
        var options = Options.Create(fixture.Create<IpStackOptions>());
        var cacheService = new Mock<ICacheService>();
        var ipStackHttpClient = new Mock<IIpStackHttpClient>();

        cacheService.Setup(cs => cs.GetFromCache<Response>(It.IsAny<string>())).Returns((Response)null);
        ipStackHttpClient.Setup(hc => hc.SendAsync<Response, ErrorResponse>(It.IsAny<HttpRequestMessage>(), It.IsAny<string>())).ReturnsAsync(errorResponse);

        var service = new LocationService(ipStackHttpClient.Object, options, cacheService.Object);

        // Act
        var result = service.GetLocation(ipLocation);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Get_Location_From_Cache()
    {
        // Arrange
        var fixture = new Fixture();
        var ipLocation = fixture.Create<IpLocation>();
        var cachedResponse = fixture.Create<Response>();
        var cacheService = new Mock<ICacheService>();
        var ipStackHttpClient = new Mock<IIpStackHttpClient>();
        var options = Options.Create(fixture.Create<IpStackOptions>());

        cacheService.Setup(cs => cs.GetFromCache<Response>(It.IsAny<string>())).Returns(cachedResponse);

        var service = new LocationService(ipStackHttpClient.Object, options, cacheService.Object);

        // Act
        var result = service.GetLocation(ipLocation);

        // Assert
        Assert.NotNull(result);
    }
}

