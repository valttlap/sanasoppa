using System.Text.Json;
using Sanasoppa.API.Data.Models;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Services;

public class ReCaptchaService : IReCaptchaService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public ReCaptchaService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task<bool> ValidateReCaptchaAsync(string token)
    {
        using var httpClient = new HttpClient();

        var secretKey = _configuration.GetValue<string>("ReCaptcha:SecretKeyDevelopment");
        var apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

        var response = await httpClient.PostAsync(apiUrl, null);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ReCaptchaResponse>(jsonResponse);

            if (result != null && result.Success)
            {
                return result.Score >= (_configuration.GetValue<double?>("ReCaptcha:MinScore") ?? 0.5);
            }
        }

        return false;
    }
}
