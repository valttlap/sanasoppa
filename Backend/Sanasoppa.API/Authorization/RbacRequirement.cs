// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;

namespace Sanasoppa.API.Authorization;

public class RbacRequirement : IAuthorizationRequirement
{
    public IEnumerable<string> Permissions { get; }

    public RbacRequirement(params string[] permissions)
    {
        Permissions = permissions;
    }
}
