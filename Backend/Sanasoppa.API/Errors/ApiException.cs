namespace Sanasoppa.API.Errors;

public class ApiException: Exception
{
    public ApiException(int statusCode, string message)
    {
        StatusCode = statusCode;
        Details = null;
    }
    public ApiException(int statusCode, string message, string? details)
    {
        StatusCode = statusCode;
        Details = details;
    }

    public int StatusCode { get; set; }
    public string? Details { get; set; }
}