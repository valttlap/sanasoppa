// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Sanasoppa.API.Authorization;
using Sanasoppa.API.Exceptions;
using Sanasoppa.API.Models.Configs;

namespace Sanasoppa.API.Extensions;

public static class IdentityServiceExtensions
{
    public static readonly RbacRequirement[] auth_admin =
    {
        new RbacRequirement("read:users"),
        new RbacRequirement("update:users"),
        new RbacRequirement("delete:users")

    };
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
            {
                var settings = config.GetSection("Auth0Settings").Get<Auth0Settings>() ?? throw new ConfigurationException("Auth0Settings is not configured");
                options.Authority = settings.Authority ?? throw new ConfigurationException("Auth0Settings.Authority is not configured");
                options.Audience = settings.Audience ?? throw new ConfigurationException("Auth0Settings.Audience is not configured");
                options.RequireHttpsMetadata = !env.IsDevelopment();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(opt =>
        {
            opt.AddPolicy("auth0admin", policy =>
            {
                foreach (var requirement in auth_admin)
                {
                    policy.Requirements.Add(requirement);
                }
            });
        });

        return services;
    }
}
