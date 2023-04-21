using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.API.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    [Required]
    public string ReCaptchaResponse { get; set; } = default!;
    public string ClientId { get; set; } = default!;
}