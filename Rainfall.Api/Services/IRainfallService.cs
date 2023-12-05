using Rainfall.Api.Domain;

namespace Rainfall.Api.Services;

public interface IRainfallService
{
    Task<IEnumerable<RainfallReading>> GetRainfallReadings(string stationId, int count);
}