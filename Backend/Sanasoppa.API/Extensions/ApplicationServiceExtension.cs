// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

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
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
            });
        });
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddSignalR();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuth0Service, Auth0Service>();
        return services;
    }
}
