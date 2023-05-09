// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
        return services;
    }
}
