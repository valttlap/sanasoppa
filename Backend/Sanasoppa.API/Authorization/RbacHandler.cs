// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;

namespace Sanasoppa.API.Authorization;

public class RbacHandler : AuthorizationHandler<RbacRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RbacRequirement requirement)
    {
        var userPermissions = context.User.FindAll(c => c.Type == "permissions").Select(c => c.Value);

        if (userPermissions == null || !userPermissions.Any())
        {
            return Task.CompletedTask;
        }

        if (requirement.Permissions.All(permission => userPermissions.Contains(permission)))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
