using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rainfall.Api.Controllers;

namespace Rainfall.Api.Tests.Controllers;

public class ErrorControllerTests
{
    [Fact]
    public async Task GetReturnsError()
    {
        var controller = new ErrorsController
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
        var feature = new Mock<IExceptionHandlerFeature>();
        feature.SetupGet(c => c.Error).Returns(new Exception("test error"));
        controller.HttpContext.Features.Set<IExceptionHandlerFeature>(feature.Object);

        // Act
        var response = controller.Error();

        // Assert
        Assert.Equal("Something went wrong during request", response.Message);
    }
}