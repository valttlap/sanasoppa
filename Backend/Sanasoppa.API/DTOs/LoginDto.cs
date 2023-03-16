using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.API.DTOs;
public class LoginDto
{
    [Required]
    public string Username { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    [Required]
    public string ReCaptchaResponse { get; set; } = default!;
}
