using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Rainfall.Api.Contracts;

namespace Rainfall.Api.Filters;

public class HttpResponseExceptionFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if(context.Exception != null)
        {

            var response = new ErrorResponse
                {
                    Message = context.Exception.Message
                };
            context.Result = new ObjectResult(response)
            {
                StatusCode = 500,
            };

            context.ExceptionHandled = true;
        }
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
}