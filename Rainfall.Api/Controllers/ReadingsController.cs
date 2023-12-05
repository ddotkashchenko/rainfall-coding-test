using Microsoft.AspNetCore.Mvc;
using Rainfall.Api.Contracts;
using Rainfall.Api.Services;

namespace Rainfall.Api.Controllers;

[ApiController]
[Route("rainfall/id/{stationId}/[controller]")]
public class ReadingsController : ControllerBase
{
    private readonly IRainfallService _rainfallService;

    public ReadingsController(IRainfallService rainfallService)
    {
        _rainfallService = rainfallService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string stationId, [FromQuery] int count = 10)
    {
        var readings = await _rainfallService.GetRainfallReadings(stationId, count);
        var response = new RainfallReadingResponse
        {
            Readings = readings.Select(r => new Contracts.RainfallReading 
            { 
                DateMeasured = r.DateMeasured, 
                AmountMeasured = r.AmountMeasured 
            })
        };
        return Ok(response);
    }
}