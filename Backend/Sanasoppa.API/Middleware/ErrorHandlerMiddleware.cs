using System.Net;
using System.Text.Json;
using Sanasoppa.API.Errors;

namespace Sanasoppa.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response is HttpResponse response && response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                await response.WriteAsJsonAsync(new
                {
                    message = "Not Found"
                });
            }
            else if (context.Response is HttpResponse unauthorizedResponse && unauthorizedResponse.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await unauthorizedResponse.WriteAsJsonAsync(new
                {
                    message = context.Request.Headers.ContainsKey("Authorization")
                                        ? "Bad credentials"
                                        : "Requires authentication"
                });
            }
            else if (context.Response is HttpResponse forbiddenResponse && forbiddenResponse.StatusCode == (int)HttpStatusCode.Forbidden)
            {
                await forbiddenResponse.WriteAsJsonAsync(new
                {
                    message = "Forbidden"
                });
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, ex.Message);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json);
    }
}