// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sanasoppa.API.Controllers;
public class AdminController : BaseApiController
{
    public AdminController()
    {
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users-with-roles")]
    public Task<ActionResult> GetUsersWithRoles()
    {
        throw new NotImplementedException();
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public Task<ActionResult> EditRoles(string username, [FromQuery] string roles)
    {
        throw new NotImplementedException();
    }

    [Authorize(Policy = "RequrireAdminRole")]
    [HttpDelete("delete-user/{username}")]
    public Task<ActionResult> DeleteUser(string username)
    {
        throw new NotImplementedException();
    }

}
