// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Exceptions;
using Sanasoppa.API.Models.Configs;

namespace Sanasoppa.API.Extensions;

public static class Auth0ServiceExtensions
{
    public static IServiceCollection AddAuth0Services(this IServiceCollection services, IConfiguration config)
    {
        var auth0Settings = config.GetSection("Auth0").Get<Auth0Settings>() ?? throw new ConfigurationException("Auth0 settings not found");
        services.AddAuth0AuthenticationClient(opt =>
        {
            opt.Domain = auth0Settings.Authority;
            opt.ClientId = auth0Settings.ClientId;
            opt.ClientSecret = auth0Settings.ClientSecret;
        });

        services.AddAuth0ManagementClient().AddManagementAccessToken();

        return services;
    }
}
