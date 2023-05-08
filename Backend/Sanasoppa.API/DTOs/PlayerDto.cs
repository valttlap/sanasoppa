// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.DTOs;
public class PlayerDto
{
    public string Name { get; set; } = default!;
    public bool IsHost { get; set; }
}
