namespace Sanasoppa.API.Interfaces;

public interface IReCaptchaService
{
    public Task<bool> ValidateReCaptchaAsync(string token);
}