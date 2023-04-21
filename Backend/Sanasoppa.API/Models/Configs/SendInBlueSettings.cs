namespace Sanasoppa.API.Models.Configs;

public class SendInBlueSettings
{
    public string ApiKey { get; set; } = default!;
    public string ApiUrl { get; set; } = default!;
    public string FromEmail { get; set; } = default!;
    public string FromName { get; set; } = default!;
    public Dictionary<string, int> Templates { get; set; } = default!;
}