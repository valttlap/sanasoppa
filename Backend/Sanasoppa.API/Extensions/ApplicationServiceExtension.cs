using Microsoft.Net.Http.Headers;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Exceptions;
using Sanasoppa.API.Interfaces;
using Sanasoppa.API.Services;

namespace Sanasoppa.API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(config["ClientUrl"] ?? throw new ConfigurationException("ClientUrl is not configured"))
                    .WithHeaders(new string[] {
                        HeaderNames.ContentType,
                        HeaderNames.Authorization,
                    })
                    .WithMethods("GET")
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
            });
        });
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IReCaptchaService, ReCaptchaService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}
