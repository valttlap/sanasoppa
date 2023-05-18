// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Controllers;

public class AdminController : BaseApiController
{
    private readonly IAuth0Service _auth0Service;

    public AdminController(IAuth0Service auth0Service)
    {
        _auth0Service = auth0Service;
    }

    [Authorize(Policy = "CanReadRoles")]
    [HttpGet("roles")]
    public async Task<ActionResult<IPagedList<Role>>> GetRoles()
    {
        return Ok(await _auth0Service.GetRolesAsync().ConfigureAwait(false));
    }

    [Authorize(Policy = "CanReadUsers")]
    [HttpGet("users")]
    public async Task<ActionResult<IPagedList<User>>> GetUsers()
    {
        return Ok(await _auth0Service.GetUsersAsync().ConfigureAwait(false));
    }

    [Authorize(Policy = "CanReadUsers")]
    [HttpGet("user/{id}")]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        return Ok(await _auth0Service.GetUserAsync(id).ConfigureAwait(false));
    }

    [Authorize(Policy = "CanReadUsers")]
    [HttpGet("user/{id}/roles")]
    public async Task<ActionResult<IPagedList<Role>>> GetUserRoles(string id)
    {
        return Ok(await _auth0Service.GetUserRolesAsync(id).ConfigureAwait(false));
    }

    [Authorize(Policy = "CanUpdateUserRoles")]
    [HttpPost("user/{id}/roles")]
    public async Task<IActionResult> AssignRolesToUser(string id, AssignRolesRequest roles)
    {
        await _auth0Service.AssignRolesToUserAsync(id, roles).ConfigureAwait(false);
        return NoContent();
    }
    [Authorize(Policy = "CanUpdateUserRoles")]
    [HttpDelete("user/{id}/roles")]
    public async Task<IActionResult> RemoveRolesFromUser(string id, AssignRolesRequest roles)
    {
        await _auth0Service.RemoveRolesFromUserAsync(id, roles).ConfigureAwait(false);
        return NoContent();
    }

    [Authorize(Policy = "CanDeleteUsers")]
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        await _auth0Service.DeletUserAsync(id).ConfigureAwait(false);
        return NoContent();
    }
}
