using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Rainfall.Api.Contracts;

namespace Rainfall.Api.Controllers;

[AllowAnonymous]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorsController : ControllerBase
{
    [Route("error")]
    public ErrorResponse Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        Response.StatusCode = 500;

        return new ErrorResponse
        {
            Message = "Something went wrong during request",
            ErrorDetails = new List<ErrorDetail>
            {
                new ErrorDetail{Message = context.Error.Message}
            }
        };
    }
}