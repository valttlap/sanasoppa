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
        context.Response.Headers.Add("X-XSS-Protection", "0");
        context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.Add("X-Frame-Options", "deny");
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; frame-ancestors 'none';");
        context.Response.Headers.Add("Cache-Control", "no-cache, no-store, max-age=0, must-revalidate");
        context.Response.Headers.Add("Pragma", "no-cache");

        await _next(context).ConfigureAwait(false);
    }
}
