using System.ComponentModel.DataAnnotations;
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
    public async Task<IActionResult> Get(string stationId, [FromQuery, Required, Range(1, 100)] int count = 10)
    {
        // if(!ModelState.IsValid)
        // {
        //     var badRequestResponse = new ErrorResponse
        //     {
        //         Message = "Bad Request",
        //         ErrorDetails = ModelState.SelectMany(v => v.Value.Errors.Select(e => 
        //             new ErrorDetail {
        //                 PropertyName = v.Key,
        //                 Message = e.ErrorMessage
        //                 }))
        //     };

        //     return BadRequest(badRequestResponse);
        // }

        var readingResult = await _rainfallService.GetRainfallReadings(stationId, count);

        if (!readingResult.Success)
        {
            if (readingResult.Errors.Any(e => e == Domain.RainfallReadingError.StationNotFound))
            {
                var errorResult = new ErrorResponse
                {
                    Message = $"Station \"{stationId}\" could not be found."
                };
                return NotFound(errorResult);
            }

            return UnprocessableEntity();
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