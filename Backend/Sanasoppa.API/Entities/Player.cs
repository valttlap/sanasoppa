// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Entities;
public class Player
{
    public int Id { get; set; }
    public string ConnectionId { get; set; } = default!;
    public string Username { get; set; } = default!;
    public bool IsOnline { get; set; }
    public bool IsHost { get; set; }
    public int TotalPoints { get; set; }
    public int? GameId { get; set; }
    public Game? Game { get; set; }
}
