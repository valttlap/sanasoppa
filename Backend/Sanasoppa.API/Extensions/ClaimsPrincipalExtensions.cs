using System.Security.Claims;

namespace Sanasoppa.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUsername(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public static int? GetUserId(this ClaimsPrincipal user)
    {
        var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return userIdString != null ? int.Parse(userIdString) : null;
    }
}
