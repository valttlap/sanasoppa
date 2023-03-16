using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sanasoppa.API.Data.Models;

public class ReCaptchaResponse
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    [JsonPropertyName("score")]
    public double Score { get; set; }
    [JsonPropertyName("action")]
    public string Action { get; set; } = default!;
    [JsonPropertyName("challenge_ts")]
    public DateTime ChallengeTs { get; set; }
    [JsonPropertyName("hostname")]
    public string Hostname { get; set; } = default!;
    [JsonPropertyName("error-codes")]
    public List<string>? ErrorCodes { get; set; }
}