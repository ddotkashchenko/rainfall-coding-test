using Microsoft.AspNetCore.Mvc;

namespace Rainfall.Api.Controllers;

[ApiController]
[Route("rainfall/id/{stationId}/[controller]")]
public class ReadingsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(string stationId, [FromQuery] int count = 10)
    {
        return Ok("readings test");
    }
}