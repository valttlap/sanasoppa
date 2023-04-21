namespace Sanasoppa.API.Models.Configs;

public class JwtSettings
{
    public string SecretKey { get; set; } = default!;
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
}