namespace Rainfall.Api.Services;

public class RainfallService
{
    private readonly HttpClient _client;
    public RainfallService(HttpClient client)
    {
        _client = client;
    }

    public async Task GetRainfallReadings(string stationId, int count)
    {
        
    }
}