using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rainfall.Api.Controllers;

namespace Rainfall.Api.Tests.Controllers;

public class ReadingsControllerTests
{
    public async Task Get_ReturnsReadingsList()
    {
        // Arrange
        var controller = new ReadingsController();

        // Act
        var result = (ObjectResult) await controller.Get("test_station");

        // Assert
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("readings test", result.Value);
    }
}