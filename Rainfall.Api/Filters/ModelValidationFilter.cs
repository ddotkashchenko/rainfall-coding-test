using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rainfall.Api.Contracts;

namespace Rainfall.Api.Filters;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.ModelState.IsValid)
        {
            var badRequestResponse = new ErrorResponse
            {
                Message = "Bad Request",
                ErrorDetails = context.ModelState.SelectMany(v => v.Value.Errors.Select(e => 
                    new ErrorDetail {
                        PropertyName = v.Key,
                        Message = e.ErrorMessage
                        }))
            };

            context.Result = new BadRequestObjectResult(badRequestResponse);
        }
    }
}