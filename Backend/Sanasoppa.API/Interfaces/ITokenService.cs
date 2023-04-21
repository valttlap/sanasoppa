using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// It takes an AppUser object and returns a token for user.
    /// </summary>
    /// <param name="AppUser">The user object that we want to create a token for.</param>
    Task<(string AccessToken, RefreshToken RefreshToken)> CreateToken(AppUser user, string clientId);
    Task<(string AccessToken, RefreshToken RefreshToken)> RefreshToken(RefreshToken refreshToken);
}
