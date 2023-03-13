using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Controllers;
public class AccountController : BaseApiController
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private const string DEFAULT_PASSWORD = "SanaSoppa2023!";

    public AccountController(UserManager<AppUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [Authorize(Policy = "RequireModeratorRole")]
    [HttpPost("add-user")]
    public async Task<ActionResult<UserDto>> AddUser(string username)
    {
        if (await UserExists(username)) return BadRequest("Username is taken");

        var user = new AppUser
        {
            UserName = username.ToLower(),
            HasDefaultPassword = true,
        };

        var result = await _userManager.CreateAsync(user, DEFAULT_PASSWORD);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            HasDefaultPassword = true
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username or password");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid usernamer or password");

        return new UserDto
        {
            Username = user.UserName!,
            Token = await _tokenService.CreateToken(user),
            HasDefaultPassword = user.HasDefaultPassword
        };
    }

    [Authorize]
    [HttpPut("update-password")]
    public async Task<ActionResult> ChangePassword(string newPassword)
    {
        var user = await _userManager.GetUserAsync(User);

        var result = await _userManager.ChangePasswordAsync(user!, user!.PasswordHash!, newPassword);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        if (user.HasDefaultPassword)
        {
            user.HasDefaultPassword = false;

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded)
            {
                return BadRequest(roleResult.Errors);
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors);
            }
        }

        return Ok();
    }


    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
