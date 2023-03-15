namespace Sanasoppa.API.DTOs;

public class ConfirmEmailDto
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;    
}