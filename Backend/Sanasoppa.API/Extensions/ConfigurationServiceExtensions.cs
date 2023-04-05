using Sanasoppa.API.Models.Configs;

namespace Sanasoppa.API.Extensions;

public static class ConfigurationServiceExtensions
{
    public static IServiceCollection AddConfigurationServices(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection("JwtSettings"));
        services.Configure<ReCaptchaSettings>(config.GetSection("ReCaptchaSettings"));
        services.Configure<SendInBlueSettings>(config.GetSection("SendInBlueSettings"));
        return services;
    }
}