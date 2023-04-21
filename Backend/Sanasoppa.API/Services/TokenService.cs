using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;
using Sanasoppa.API.Models.Configs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sanasoppa.API.Services;

public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSettings _jwtSettings;
    private readonly IUnitOfWork _uow;

    public TokenService(
        IOptions<JwtSettings> jwtSettings,
        UserManager<AppUser> userManager,
        IUnitOfWork uow
        )
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            _jwtSettings.SecretKey ?? throw new Exception("Tokenkey not found")));
        _uow = uow;
    }

    public async Task<(string AccessToken, RefreshToken RefreshToken)> CreateToken(AppUser user, string clientId)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
        };

        var roles = await _userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var now = DateTime.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            SigningCredentials = creds,
            NotBefore = now
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);

        var refreshToken = GenerateRefreshToken(user, clientId);

        return (accessToken, refreshToken);
    }

    public async Task<(string AccessToken, RefreshToken RefreshToken)> RefreshToken(RefreshToken refreshToken)
    {
        if (!await _uow.RefreshTokenRepository.ValidateRefreshTokenAsync(refreshToken))
        {
            throw new SecurityTokenException("Invalid refresh token");
        }
        await _uow.RefreshTokenRepository.RevokeRefreshTokenAsync(refreshToken);
        return await CreateToken(refreshToken.User, refreshToken.ClientId);
    }

    private RefreshToken GenerateRefreshToken(AppUser user, string clientId)
    {
        byte[] randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        var token = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: Convert.ToBase64String(randomNumber),
            salt: Array.Empty<byte>(), // No salt is needed since the input is a random number
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 32));

        return new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            Expires = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            Revoked = false,
            User = user,
            ClientId = clientId,
        };
    }
}
