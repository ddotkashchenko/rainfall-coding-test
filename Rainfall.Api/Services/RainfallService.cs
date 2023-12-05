using Rainfall.Api.Domain;

namespace Rainfall.Api.Services;

public class RainfallService : IRainfallService
{
    private readonly HttpClient _client;
    public RainfallService(HttpClient client)
    {
        _client = client;
    }

    public async Task<RainfallReadingResult> GetRainfallReadings(string stationId, int count)
    {
        var url = $"https://environment.data.gov.uk/flood-monitoring/id/stations/{stationId}/readings?_sorted&_limit={count}";
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