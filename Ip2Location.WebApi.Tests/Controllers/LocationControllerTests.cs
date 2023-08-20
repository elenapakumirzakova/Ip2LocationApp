namespace Ip2Location.WebApi.Tests.Controllers;

public class LocationControllerTests
{
    [Fact]
    public async Task Get_WithValidId_ReturnsOkAsync()
    {
        // Arrange
        var fixture = new Fixture();
        var locationService = new Mock<ILocationService>();
        var repository = new Mock<IRepository<IpLocation>>();
        var ip = fixture.Create<IPAddress>().ToString();
        var ipLocationRequest = fixture.Create<IpLocation>();
        var response = OneOf<IpLocation, ErrorResponse>.FromT0(fixture.Create<IpLocation>());

        locationService.Setup(cs => cs.GetLocation(It.IsAny<IpLocation>())).ReturnsAsync(response);
        repository.Setup(hc => hc.InsertAsync(It.IsAny<IpLocation>()));

        var controller = new LocationController(locationService.Object, repository.Object);

        // Act
        var result = await controller.Get(ip);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var responseModel = okResult.Value as ResponseModel;
        Assert.Equal(response.AsT0.Ip, responseModel.Ip);
        Assert.Equal(response.AsT0.Country, responseModel.Country);
    }

    [Fact]
    public async Task Get_WithInvalidId_ReturnsErrorAsync()
    {
        // Arrange
        var fixture = new Fixture();
        var locationService = new Mock<ILocationService>();
        var repository = new Mock<IRepository<IpLocation>>();
        var ip = fixture.Create<string>();
        var ipLocationRequest = fixture.Create<IpLocation>();
        var response = OneOf<Response, ErrorResponse>.FromT1(fixture.Create<ErrorResponse>());

        var controller = new LocationController(locationService.Object, repository.Object);

        // Act
        var result = await controller.Get(ip);

        // Assert
        var requestResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(422, requestResult.StatusCode);
        var responseModel = requestResult.Value as ErrorResponseModel;
        Assert.Equal("validation_error", responseModel.Type);
        Assert.Equal("IP address is invalid.", responseModel.Info);
    }
}

