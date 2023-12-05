using Rainfall.Api.Domain;

namespace Rainfall.Api.Services;

public class RainfallService : IRainfallService
{
    private readonly HttpClient _client;
    public RainfallService(HttpClient client)
    {
        _client = client;
    }

    public async Task<IEnumerable<RainfallReading>> GetRainfallReadings(string stationId, int count)
    {
        return new List<RainfallReading>{new RainfallReading()};
    }
}