// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Auth0.ManagementApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Sanasoppa.API.Controllers;
public class AdminController : BaseApiController
{
    private readonly IManagementApiClient _managementApiClient;

    public AdminController(IManagementApiClient managementApiClient)
    {
        _managementApiClient = managementApiClient;
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpGet("users")]
    public Task<ActionResult> GetUsersWithRoles()
    {
         var users = await _managementApiClient.Users.GetAllAsync(new GetUsersRequest(), new PaginationInfo());
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
