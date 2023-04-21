using System.Net;
using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly IReCaptchaService _reCaptchaService;
    private readonly IUnitOfWork _uow;

    public AccountController(
        UserManager<AppUser> userManager,
        ITokenService tokenService,
        IMapper mapper,
        IEmailService emailService,
        IReCaptchaService reCaptchaService,
        IUnitOfWork uow)
    {
        _reCaptchaService = reCaptchaService;
        _emailService = emailService;
        _mapper = mapper;
        _userManager = userManager;
        _tokenService = tokenService;
        _uow = uow;
    }

    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> AddUser(RegisterDto registerDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _reCaptchaService.ValidateReCaptchaAsync(registerDto.ReCaptchaResponse))
        {
            return BadRequest("Invalid ReCaptcha");
        }
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        var user = _mapper.Map<AppUser>(registerDto);

        user.UserName = registerDto.Username.ToLower();
        user.Email = registerDto.Email.ToLower();

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        var newTokens = await _tokenService.CreateToken(user, registerDto.ClientId);

        _uow.RefreshTokenRepository.AddRefreshToken(newTokens.RefreshToken);

        if (!await _uow.Complete())
        {
            return BadRequest("Failed to add refresh token");
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailResult = await _emailService.SendConfirmationEmailAsync(user.Email, token);

        return new UserDto
        {
            Username = user.UserName,
            Token = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken.Token,
            RefreshTokenExpiration = newTokens.RefreshToken.Expires,
        };
    }

    [HttpPost("confirm-email")]
    public async Task<ActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _reCaptchaService.ValidateReCaptchaAsync(confirmEmailDto.ReCaptchaResponse))
        {
            return BadRequest("Invalid ReCaptcha");
        }
        var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);

        if (user == null) return BadRequest("Invalid email");

        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    [Authorize]
    [HttpPost("resend-email-confirm")]
    public async Task<ActionResult> ResendEmailConfirm()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null) return BadRequest("User not found");

        if (user.EmailConfirmed) return BadRequest("Email already confirmed");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var emailResult = await _emailService.SendConfirmationEmailAsync(user.Email!, token);

        if (!emailResult)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send email. Try again later or contact support.");
        }

        return Ok();
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _reCaptchaService.ValidateReCaptchaAsync(forgotPasswordDto.ReCaptchaResponse))
        {
            return BadRequest("Invalid ReCaptcha");
        }
        var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

        if (user == null) return Ok();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var emailResult = await _emailService.SendPasswordResetEmailAsync(user.Email!, token);

        if (!emailResult)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "Failed to send email. Try again later or contact support.");
        }

        return Ok();
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
    {
        var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);

        if (user == null) return BadRequest("User not found");

        var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }


    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (!await _reCaptchaService.ValidateReCaptchaAsync(loginDto.ReCaptchaResponse))
        {
            return BadRequest("Invalid ReCaptcha");
        }
        var user = await _userManager.Users
            .SingleOrDefaultAsync(x => x.UserName == loginDto.Username);

        if (user == null) return Unauthorized("Invalid username or password");

        var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!result) return Unauthorized("Invalid usernamer or password");

        var newTokens = await _tokenService.CreateToken(user, loginDto.ClientId);

        return new UserDto
        {
            Username = user.UserName!,
            Token = newTokens.AccessToken,
            RefreshToken = newTokens.RefreshToken.Token,
            RefreshTokenExpiration = newTokens.RefreshToken.Expires,
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

        return Ok();
    }


    private async Task<bool> UserExists(string username)
    {
        return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
