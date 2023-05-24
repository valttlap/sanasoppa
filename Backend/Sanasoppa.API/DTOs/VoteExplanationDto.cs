// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.DTOs;
public class VoteExplanationDto
{
    public int Id { get; set; }
    public string Explanation { get; set; } = default!;
}
