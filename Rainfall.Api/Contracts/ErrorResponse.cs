namespace Rainfall.Api.Contracts;

public class ErrorResponse
{
    public string Message { get; set; }
    public IEnumerable<ErrorDetail> ErrorDetails { get; set; }
}