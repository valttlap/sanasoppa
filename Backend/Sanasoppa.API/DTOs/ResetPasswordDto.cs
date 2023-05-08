// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.DTOs;

public class ResetPasswordDto
{
    public string Email { get; set; } = default!;
    public string Token { get; set; } = default!;
    public string Password { get; set; } = default!;
}
