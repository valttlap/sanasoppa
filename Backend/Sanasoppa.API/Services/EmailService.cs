// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Interfaces;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Sanasoppa.API.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;
    private readonly TransactionalEmailsApi _apiInstance;
    // TODO: ADD logging and token handling
    public EmailService(IConfiguration config)
    {
        _config = config;
        var apiKey = _config.GetValue<string>("SendGrid:ApiKey");
        Configuration.Default.AddApiKey("api-key", apiKey);
        _apiInstance = new TransactionalEmailsApi();
    }

    public async Task<bool> SendConfirmationEmailAsync(string email, string token)
    {
        return await SendEmailAsync(email, "SendGrid:Templates:Confirmation").ConfigureAwait(false);
    }

    public async Task<bool> SendPasswordResetEmailAsync(string email, string token)
    {
        return await SendEmailAsync(email, "SendGrid:Templates:PasswordReset").ConfigureAwait(false);
    }

    private async Task<bool> SendEmailAsync(string email, string templatePath)
    {
        var templateId = _config.GetValue<int>(templatePath);
        var fromEmail = _config.GetValue<string>("SendGrid:SenderEmail");
        var fromName = _config.GetValue<string>("SendGrid:SenderName");

        var sendSmtpEmail = new SendSmtpEmail(
            sender: new SendSmtpEmailSender(fromEmail, fromName),
            to: new List<SendSmtpEmailTo> { new SendSmtpEmailTo(email) },
            templateId: templateId
        );

        try
        {
            await _apiInstance.SendTransacEmailAsync(sendSmtpEmail).ConfigureAwait(false);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}
