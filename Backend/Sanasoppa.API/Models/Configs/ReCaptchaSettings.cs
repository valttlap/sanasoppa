// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Models.Configs;

public class ReCaptchaSettings
{
    public string SecretKey { get; set; } = default!;
    public double? MinValidScore { get; set; }
}
