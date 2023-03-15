using Microsoft.AspNetCore.Mvc;

namespace Sanasoppa.API.Interfaces;

public interface IEmailService
{
    public Task<bool> SendConfirmationEmailAsync(string email, string token);
    public Task<bool> SendPasswordResetEmailAsync(string email, string token);   
}