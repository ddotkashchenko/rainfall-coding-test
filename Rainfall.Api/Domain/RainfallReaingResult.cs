namespace Rainfall.Api.Domain;

public class RainfallReadingResult
{
    public bool Success { get; set; }
    public IEnumerable<RainfallReading> Readings { get; set; }
    public IEnumerable<RainfallReadingError> Errors { get; set; }
}