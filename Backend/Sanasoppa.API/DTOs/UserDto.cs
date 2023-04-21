namespace Sanasoppa.API.DTOs;
public class UserDto
{
    public string Username { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string RefreshToken { get; set; } = default!;
    public DateTime RefreshTokenExpiration { get; set; }
}
