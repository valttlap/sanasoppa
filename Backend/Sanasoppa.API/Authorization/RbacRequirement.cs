// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;

namespace Sanasoppa.API.Authorization;

public class RbacRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public RbacRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}
