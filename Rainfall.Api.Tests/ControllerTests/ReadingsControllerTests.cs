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
            .ReturnsAsync(GetReadings());
        var controller = new ReadingsController(rainfallServiceMock.Object);

        // Act
        var result = await controller.Get("test_station");

        // Assert
        var objectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

        var readingsResponse = Assert.IsAssignableFrom<RainfallReadingResponse>(objectResult.Value);
        Assert.Equal(3, readingsResponse.Readings.Count());
    }

    private List<Domain.RainfallReading> GetReadings()
    {
        return new List<Domain.RainfallReading>
        {
            new Domain.RainfallReading(){DateMeasured = DateTime.Now, AmountMeasured = 10},
            new Domain.RainfallReading(){DateMeasured = DateTime.Now, AmountMeasured = 10},
            new Domain.RainfallReading(){DateMeasured = DateTime.Now, AmountMeasured = 10}
        };
    }
}