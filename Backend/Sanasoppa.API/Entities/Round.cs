// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Entities;
public class Round
{
    public Round()
    {
        Explanations = new List<Explanation>();
        Votes = new List<Vote>();
    }
    public int Id { get; set; }
    public bool IsCurrent { get; set; }
    public int DasherId { get; set; }
    public string Word { get; set; } = default!;
    public int GameId { get; set; }
    public Game Game { get; set; } = default!;
    public Player Dasher { get; set; } = default!;
    public ICollection<Explanation> Explanations { get; set; }
    public ICollection<Vote> Votes { get; set; }
}
