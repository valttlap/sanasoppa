// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Security.Claims;

namespace Sanasoppa.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst("https://sanasoppa.fi/nickname")?.Value;
    }
    public static string? GetUserId(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
