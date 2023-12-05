using Rainfall.Api.Domain;

namespace Rainfall.Api.Services;

public class RainfallService : IRainfallService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public RainfallService(HttpClient client, IConfiguration configuration)
    {
        _client = client;
        _baseUrl = configuration["RainfallFloodMonitoringBaseUrl"];
    }

    public async Task<RainfallReadingResult> GetRainfallReadings(string stationId, int count)
    {
        var url = $"{_baseUrl}id/stations/{stationId}/readings?_sorted&_limit={count}";
        using var httpResponseMessage = await _client.GetAsync(url);

        if(httpResponseMessage.IsSuccessStatusCode)
        {
            var readings = await httpResponseMessage.Content.ReadFromJsonAsync<RainfallReadingResponse>();

            if(readings.Items.Any())
            {
                return new RainfallReadingResult 
                {
                    Success = true,
                    Readings = readings.Items
                };
            }
            else // Assume no items = wrong station id (while testing source API sending -5 returns empty items)
            {
                return new RainfallReadingResult 
                {
                    Success = false,
                    Errors = new List<RainfallReadingError> {RainfallReadingError.StationNotFound}
                };
            }
        }
        return new RainfallReadingResult 
        {
            Success = false,
            Errors = new List<RainfallReadingError> {RainfallReadingError.RequestError}
        };
    }
}