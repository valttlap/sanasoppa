// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Sanasoppa.API.Interfaces;
using Sanasoppa.API.Responses.Models;

namespace Sanasoppa.API.Services;

public class ReCaptchaService : IReCaptchaService
{
    private readonly IConfiguration _configuration;

    public ReCaptchaService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ValidateReCaptchaAsync(string token)
    {
        using var httpClient = new HttpClient();

        var secretKey = _configuration.GetValue<string>("ReCaptcha:SecretKeyDevelopment");
        var apiUrl = $"https://www.google.com/recaptcha/api/siteverify?secret={secretKey}&response={token}";

        var response = await httpClient.PostAsync(apiUrl, null).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var result = JsonSerializer.Deserialize<ReCaptchaResponse>(jsonResponse);

            if (result != null && result.Success)
            {
                return result.Score >= (_configuration.GetValue<double?>("ReCaptcha:MinScore") ?? 0.5);
            }
        }

        return false;
    }
}
