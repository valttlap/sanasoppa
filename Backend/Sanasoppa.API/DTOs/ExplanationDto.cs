// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.DTOs;
public class ExplanationDto
{
    public int PlayerId { get; set; }
    public string PlayerName { get; set; } = default!;
    public string Explanation { get; set; } = default!;
}
