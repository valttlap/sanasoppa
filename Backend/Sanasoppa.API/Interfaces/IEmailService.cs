// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Interfaces;

public interface IEmailService
{
    public Task<bool> SendConfirmationEmailAsync(string email, string token);
    public Task<bool> SendPasswordResetEmailAsync(string email, string token);
}
