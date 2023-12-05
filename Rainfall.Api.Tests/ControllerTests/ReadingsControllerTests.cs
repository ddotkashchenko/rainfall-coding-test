using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rainfall.Api.Contracts;
using Rainfall.Api.Controllers;
using Rainfall.Api.Domain;
using Rainfall.Api.Services;

namespace Rainfall.Api.Tests.Controllers;

public class ReadingsControllerTests
{
    [Fact]
    public async Task Get_ReturnsReadingsList()
    {
        // Arrange
        var rainfallServiceMock = new Mock<IRainfallService>();
        rainfallServiceMock
            .Setup(s => s.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(GetSuccessReadingsResult());
        var controller = new ReadingsController(rainfallServiceMock.Object);

        // Act
        var result = await controller.Get("test_station");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var readingsResponse = Assert.IsAssignableFrom<RainfallReadingResponse>(okResult.Value);
        Assert.Equal(3, readingsResponse.Readings.Count());
    }

    [Fact]
    public async Task Get_ReturnsError404StationNotFound()
    {
        // Arrange
        var rainfallServiceMock = new Mock<IRainfallService>();
        rainfallServiceMock
            .Setup(s => s.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(GetStationNotFoundResult());
        var controller = new ReadingsController(rainfallServiceMock.Object);

        // Act
        var result = await controller.Get("test_station");

        // Assert
        var notfoudResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notfoudResult.StatusCode);

        var errorResponse = Assert.IsAssignableFrom<ErrorResponse>(notfoudResult.Value);
        Assert.Equal("Station \"test_station\" could not be found.", errorResponse.Message);
    }

    [Fact]
    public async Task Get_ReturnsError400BadRequest()
    {
        // Arrange
        var rainfallServiceMock = new Mock<IRainfallService>();
        rainfallServiceMock
            .Setup(s => s.GetRainfallReadings(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(GetSuccessReadingsResult());
        var controller = new ReadingsController(rainfallServiceMock.Object);
        controller.ModelState.AddModelError("count", "Exceeds range");

        // Act
        var result = await controller.Get("test_station", -1);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    private RainfallReadingResult GetSuccessReadingsResult()
    {
        return new RainfallReadingResult 
        {
            Success = true,
            Readings = new List<Domain.RainfallReading>
            {
                new Domain.RainfallReading {DateMeasured = DateTime.Now, AmountMeasured = 1},
                new Domain.RainfallReading {DateMeasured = DateTime.Now, AmountMeasured = 100},
                new Domain.RainfallReading {DateMeasured = DateTime.Now, AmountMeasured = 1000}
            }
        };
    }

    private RainfallReadingResult GetStationNotFoundResult()
    {
        return new RainfallReadingResult
        {
            Success = false,
            Errors = new List<RainfallReadingError> {RainfallReadingError.StationNotFound}
        };
    }
}