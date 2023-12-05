using Rainfall.Api.Domain;

namespace Rainfall.Api.Services;

public interface IRainfallService
{
    Task<RainfallReadingResult> GetRainfallReadings(string stationId, int count);
}