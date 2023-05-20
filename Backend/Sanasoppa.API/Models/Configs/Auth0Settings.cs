// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Models.Configs;

public class Auth0Settings
{
    public string Authority { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string ClientId { get; set; } = default!;
    public string ClientSecret { get; set; } = default!;
    public string ManagementAudience { get; set; } = default!;
}
