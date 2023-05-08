using Sanasoppa.API.Middleware;

namespace Sanasoppa.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecureHeadersMiddleware>();
    }

    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}