using System.Text.Json.Serialization;

namespace Rainfall.Api.Domain;

public class RainfallReading
{
    [JsonPropertyName("dateTime")]
    public DateTime DateMeasured { get; set; }
    [JsonPropertyName("value")]
    public decimal AmountMeasured { get; set; }
}