using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Rainfall.Api.Contracts;
using Rainfall.Api.Services;

namespace Rainfall.Api.Controllers;

[ApiController]
[Route("rainfall/id/{stationId}/[controller]")]
[Produces("application/json")]
public class ReadingsController : ControllerBase
{
    private readonly IRainfallService _rainfallService;

    public ReadingsController(IRainfallService rainfallService)
    {
        _rainfallService = rainfallService;
    }

    /// <summary>
    /// Get rainfall readings by station Id
    /// </summary>
    /// <remarks>
    /// Retrieve the latest readings for the specified stationId
    /// </remarks>
    /// <param name="stationId">The id of the reading station</param>
    /// <param name="count">The number of readings to return</param>
    /// <response code="200">A list of rainfall readings successfully retrieved</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">No readings found for the specified stationId</response>
    /// <response code="500">Internal server error</response>
    [HttpGet]
    [ProducesResponseType(typeof(RainfallReadingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(string stationId, [FromQuery, Range(1, 100)] int count = 10)
    {
        var readingResult = await _rainfallService.GetRainfallReadings(stationId, count);

        if (!readingResult.Success)
        {
            if (readingResult.Errors.Any(e => e == Domain.RainfallReadingError.StationNotFound))
            {
                var stationNotFoundResult = new ErrorResponse
                {
                    Message = $"Station \"{stationId}\" could not be found."
                };
                return NotFound(stationNotFoundResult);
            }

            var errorResult = new ErrorResponse
            {
                Message = "Response could not be processed."
            };

            return BadRequest(errorResult);
        }

        var response = new RainfallReadingResponse
        {
            Readings = readingResult.Readings.Select(r => new Contracts.RainfallReading
            {
                DateMeasured = r.DateMeasured,
                AmountMeasured = r.AmountMeasured
            })
        };
        return Ok(response);
    }
}