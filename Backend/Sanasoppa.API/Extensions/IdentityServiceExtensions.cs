// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Sanasoppa.API.Authorization;
using Sanasoppa.API.Exceptions;
using Sanasoppa.API.Models.Configs;

namespace Sanasoppa.API.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
            {
                var settings = config.GetSection("Auth0").Get<Auth0Settings>() ?? throw new ConfigurationException("Auth0Settings is not configured");
                options.Authority = settings.Authority ?? throw new ConfigurationException("Auth0Settings.Authority is not configured");
                options.Audience = settings.Audience ?? throw new ConfigurationException("Auth0Settings.Audience is not configured");
                options.RequireHttpsMetadata = !env.IsDevelopment();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanCreateUsers", policy => policy.Requirements.Add(new RbacRequirement(new[] { "create:users" })));
            options.AddPolicy("CanDeleteUsers", policy => policy.Requirements.Add(new RbacRequirement(new[] { "delete:users" })));
            options.AddPolicy("CanReadUsers", policy => policy.Requirements.Add(new RbacRequirement(new[] { "read:users" })));
            options.AddPolicy("CanUpdateUsers", policy => policy.Requirements.Add(new RbacRequirement(new[] { "update:users" })));
            options.AddPolicy("CanReadRoles", policy => policy.Requirements.Add(new RbacRequirement(new[] { "read:roles" })));
            options.AddPolicy("CanUpdateUserRoles", policy => policy.Requirements.Add(new RbacRequirement(new[] { "update:roles", "update:users" })));
        });

        services.AddSingleton<IAuthorizationHandler, RbacHandler>();

        return services;
    }
}
