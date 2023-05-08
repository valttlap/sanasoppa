// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Sanasoppa.API.DTOs;

public class ConfirmEmailDto
{
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Token { get; set; } = default!;
    [Required]
    public string ReCaptchaResponse { get; set; } = default!;
}