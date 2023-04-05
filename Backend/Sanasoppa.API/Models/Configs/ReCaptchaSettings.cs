namespace Sanasoppa.API.Models.Configs;

public class ReCaptchaSettings
{
    public string SecretKey { get; set; } = default!;
    public double? MinValidScore { get; set; }
}