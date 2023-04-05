using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DataContext _context;
    public RefreshTokenRepository(DataContext context)
    {
        _context = context;
    }

    public void AddRefreshToken(RefreshToken refreshToken)
    {
        refreshToken.Token = hashToken(refreshToken.Token);
        _context.RefreshTokens.Add(refreshToken);
    }

    public async Task<bool> RevokeAllUserRefreshTokensAsync(int userId)
    {
        var tokens = _context.RefreshTokens
            .Where(x => x.UserId == userId);
        foreach (var token in tokens)
        {
            token.Revoked = true;
        }
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
    {
        var token = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == refreshToken)
            .ConfigureAwait(false);
        if (token == null)
        {
            return false;
        }

        if (token.Revoked)
        {
            return true;
        }

        token.Revoked = true;
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> RevokeRefreshTokenAsync(RefreshToken refreshToken)
    {
        if (refreshToken == null)
        {
            return false;
        }

        if (refreshToken.Revoked)
        {
            return true;
        }

        refreshToken.Revoked = true;
        return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
    }

    public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, int userId, string clientId)
    {
        var hashedToken = hashToken(refreshToken);
        var token = await _context.RefreshTokens
            .SingleOrDefaultAsync(x => x.Token == hashedToken && x.UserId == userId && x.ClientId == clientId)
            .ConfigureAwait(false);
        if (token == null)
        {
            return false;
        }
        var isValid = !token.IsExpired && !token.Revoked;
        if (!isValid)
        {
            await RevokeRefreshTokenAsync(token).ConfigureAwait(false);
            return false;
        }
        return true;
    }

    private string hashToken(string token)
    {
        return Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: token,
                salt: Array.Empty<byte>(),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32)
            );
    }
}