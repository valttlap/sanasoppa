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
        services.Configure<Auth0Settings>(config.GetSection("Auth0"));
        return services;
    }
}
