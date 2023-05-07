using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Exceptions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Middleware;

public class TokenRefreshMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;

    public TokenRefreshMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env, ITokenService tokenService, IConfiguration config)
    {
        _next = next;
        _logger = logger;
        _env = env;
        _tokenService = tokenService;
        _config = config;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        var accessToken = httpContext.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrWhiteSpace(accessToken))
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding
                        .UTF8.GetBytes(_config["TokenKey"] ?? throw new ConfigurationException("Token key not found"))),
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                try
                {
                    var refreshToken = httpContext.Request.Cookies["refreshToken"];
                    if (refreshToken == null)
                    {
                        throw new SecurityTokenException("Refresh token not found.");
                    }
                    RefreshToken? deserializedRefreshToken = null;
                    try
                    {
                        deserializedRefreshToken = JsonSerializer.Deserialize<RefreshToken>(refreshToken);
                    }
                    catch (JsonException)
                    {
                        throw new SecurityTokenException("Refresh token is invalid.");
                    }

                    if (deserializedRefreshToken == null)
                    {
                        throw new SecurityTokenException("Refresh token is invalid.");
                    }
                    var (newAccessToken, newRefreshToken) = await _tokenService.RefreshToken(deserializedRefreshToken);

                    httpContext.Request.Headers["Authorization"] = "Bearer " + newAccessToken;

                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = newRefreshToken.Expires,
                        SameSite = _env.IsDevelopment() ? SameSiteMode.None : SameSiteMode.Strict,
                        Secure = _env.IsDevelopment() ? false : true,
                        Domain = _env.IsDevelopment() ? null : _config["Domain"] ?? throw new ConfigurationException("Domain setting not found")
                    };

                    httpContext.Response.Cookies.Append("refreshToken", JsonSerializer.Serialize(newRefreshToken), cookieOptions);
                }
                catch (SecurityTokenException e)
                {
                    _logger.LogError(e, "Refresh token error");
                }

            }

        }
        await _next(httpContext);
    }
}