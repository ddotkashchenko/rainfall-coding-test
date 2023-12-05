
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Moq;
using Rainfall.Api.Contracts;
using Rainfall.Api.Filters;

namespace Rainfall.Api.Tests.Filters;

public class ModelValidationFilterTests
{
    [Fact]
    public async Task FiltersModelValidationError()
    {
        // Arrange
        var context = GetContextWithModelError();
        var filter = new ModelValidationFilter();

        // Act
        filter.OnActionExecuting(context);

        var barRequestResult = Assert.IsAssignableFrom<BadRequestObjectResult>(context.Result);
        var errorResponse = Assert.IsAssignableFrom<ErrorResponse>(barRequestResult.Value);
        Assert.Equal(1, errorResponse.ErrorDetails.Count());

        var errorDetail = errorResponse.ErrorDetails.First();
        Assert.Equal("count", errorDetail.PropertyName);
        Assert.Equal("invalid range", errorDetail.Message); 
    }

    [Fact]
    public async Task PassesValidModel()
    {
        // Arrange
        var context = GetContextWithValidModel();
        var filter = new ModelValidationFilter();

        // Act
        filter.OnActionExecuting(context);
        Assert.Null(context.Result);
    }

    private ActionExecutingContext GetContextWithModelError()
    {
        var modelState = new ModelStateDictionary();
        modelState.AddModelError("count", "invalid range");

        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<Microsoft.AspNetCore.Routing.RouteData>(),
            Mock.Of<ActionDescriptor>(),
            modelState
        );

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            Mock.Of<Controller>()
        );
    }
    
    private ActionExecutingContext GetContextWithValidModel()
    {
        var modelState = new ModelStateDictionary();
        var actionContext = new ActionContext(
            Mock.Of<HttpContext>(),
            Mock.Of<Microsoft.AspNetCore.Routing.RouteData>(),
            Mock.Of<ActionDescriptor>(),
            modelState
        );

        return new ActionExecutingContext(
            actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object>(),
            Mock.Of<Controller>()
        );
    } 
}