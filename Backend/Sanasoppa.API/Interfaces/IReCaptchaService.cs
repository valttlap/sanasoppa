// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Interfaces;

public interface IReCaptchaService
{
    public Task<bool> ValidateReCaptchaAsync(string token);
}
