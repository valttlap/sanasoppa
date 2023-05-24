// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Middleware;

public class SecureHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecureHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Prevents reflected Cross-Site Scripting (XSS) attacks in older browsers.
        context.Response.Headers.Add("X-XSS-Protection", "0");

        // Ensures that the website can only be accessed using HTTPS, preventing man-in-the-middle attacks.
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");

        // Prevents the website from being embedded within an iframe, which helps protect against clickjacking attacks.
        context.Response.Headers.Add("X-Frame-Options", "deny");

        // Prevents browsers from interpreting files as a different MIME type, reducing the risk of MIME-sniffing attacks.
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

        // Specifies allowed sources for content, reducing the risk of cross-site scripting (XSS) and other code injection attacks.
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none';");

        // Disables caching of the response, ensuring sensitive information is not stored in the browser cache.
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");

        // Disables caching in HTTP/1.0 clients, ensuring sensitive information is not stored in the browser cache.
        context.Response.Headers.Add("Pragma", "no-cache");

        await _next(context).ConfigureAwait(false);
    }
}
