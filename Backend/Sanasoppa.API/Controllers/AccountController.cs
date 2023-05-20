// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.RegularExpressions;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Controllers;

[Authorize]
public partial class AccountController : BaseApiController
{
    private readonly IAuth0Service _auth0Service;

    public AccountController(IAuth0Service auth0Service)
    {
        _auth0Service = auth0Service;
    }

    /// <summary>
    /// Get specific user
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns>User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     GET /api/account/user/1234567890
    ///
    /// </remarks>
    /// <response code="200">Returns user</response>
    /// <response code="401">Unauthorized if not requesting own data</response>
    [HttpGet("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> GetUser(string id)
    {
        if (id != User.GetUserId())
        {
            return Unauthorized();
        }

        return Ok(await _auth0Service.GetUserAsync(id).ConfigureAwait(false));
    }

    /// <summary>
    /// Update specific user's username
    /// </summary>
    /// <param name="id">User id</param>
    /// <param name="name">New username</param>
    /// <returns>User</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PATCH /api/account/user/1234567890?name=NewUsername
    ///
    /// </remarks>
    /// <response code="200">Returns user</response>
    /// <response code="400">Bad request if invalid characters in name</response>
    /// <response code="401">Unauthorized if not updating own data</response>
    [HttpPatch("user/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<User>> UpdateUserName(string id, [FromQuery] string name)
    {
        if (id != User.GetUserId())
        {
            return Unauthorized();
        }

        if (InvalidCharacters().IsMatch(name))
        {
            return BadRequest("Invalid characters in name");
        }

        return Ok(await _auth0Service.UpdateUserNameAsync(id, name).ConfigureAwait(false));
    }

    /// <summary>
    /// Resend verification email to specific user
    /// </summary>
    /// <param name="id">User id</param>
    /// <returns></returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/account/user/1234567890/resend-verification
    ///
    /// </remarks>
    /// <response code="204">Email sent</response>
    /// <response code="401">Unauthorized if not requesting own data</response>
    [HttpPost("user/{id}/resend-verification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ResendUserVerificationEmail(string id)
    {
        if (id != User.GetUserId())
        {
            return Unauthorized();
        }
        await _auth0Service.ResendUserVerificationEmailAsync(id).ConfigureAwait(false);
        return NoContent();
    }

    [GeneratedRegex(@"[^\w._@\-öÖäÄåÅ]")]
    private static partial Regex InvalidCharacters();
}
