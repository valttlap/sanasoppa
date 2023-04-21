using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IRefreshTokenRepository
{
    void AddRefreshToken(RefreshToken refreshToken);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken, int userId, string clientId);
    Task<bool> ValidateRefreshTokenAsync(RefreshToken refreshToken);
    Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    Task<bool> RevokeRefreshTokenAsync(RefreshToken refreshToken);
    Task<bool> RevokeAllUserRefreshTokensAsync(int userId);
}