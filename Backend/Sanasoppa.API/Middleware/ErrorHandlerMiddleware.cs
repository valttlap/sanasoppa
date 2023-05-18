// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Net;
using System.Text.Json;
using Sanasoppa.API.Errors;

namespace Sanasoppa.API.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;

    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context).ConfigureAwait(false);

            if (!context.Response.HasStarted)
            {
                if (context.Response is HttpResponse response && response.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    await response.WriteAsJsonAsync(new
                    {
                        message = "Not Found"
                    }).ConfigureAwait(false);
                }
                else if (context.Response is HttpResponse unauthorizedResponse && unauthorizedResponse.StatusCode == (int)HttpStatusCode.Unauthorized)
                {
                    await unauthorizedResponse.WriteAsJsonAsync(new
                    {
                        message = context.Request.Headers.ContainsKey("Authorization")
                                            ? "Bad credentials"
                                            : "Requires authentication"
                    }).ConfigureAwait(false);
                }
                else if (context.Response is HttpResponse forbiddenResponse && forbiddenResponse.StatusCode == (int)HttpStatusCode.Forbidden)
                {
                    await forbiddenResponse.WriteAsJsonAsync(new
                    {
                        message = "Forbidden"
                    }).ConfigureAwait(false);
                }
            }
            else
            {
                // Log the issue or handle it as appropriate for your application
                _logger.LogWarning("Response has already started, unable to write custom JSON error message.");
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex).ConfigureAwait(false);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "An exception occurred: {ExceptionMessage}", ex.Message);
        if (context.Response.HasStarted)
        {
            return;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = _env.IsDevelopment()
                ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                : new ApiException(context.Response.StatusCode, "Internal Server Error");

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var json = JsonSerializer.Serialize(response, options);

        await context.Response.WriteAsync(json).ConfigureAwait(false);
    }
}
