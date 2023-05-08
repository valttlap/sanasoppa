namespace Sanasoppa.Application.DTOs;

public class ResetPasswordDto
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}
