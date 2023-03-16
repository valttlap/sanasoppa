using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Services;

public class EmailService : IEmailService
{
    public Task<bool> SendConfirmationEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SendPasswordResetEmailAsync(string email, string token)
    {
        throw new NotImplementedException();
    }
}