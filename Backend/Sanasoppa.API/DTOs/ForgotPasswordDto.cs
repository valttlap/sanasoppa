using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.API.DTOs;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = default!;
    [Required]
    public string ReCaptchaResponse { get; set; } = default!;
}