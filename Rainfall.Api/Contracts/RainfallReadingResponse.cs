namespace Rainfall.Api.Contracts;

public class RainfallReadingResponse
{
    public IEnumerable<RainfallReading> Readings {get;set;}
}