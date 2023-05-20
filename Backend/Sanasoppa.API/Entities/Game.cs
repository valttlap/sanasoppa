// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Sanasoppa.API.Entities;
public class Game
{
    public Game()
    {
        Players = new List<Player>();
        Rounds = new List<Round>();
    }
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool HasStarted => GameState != GameState.NotStarted;
    public GameState GameState { get; set; }
    public Round? CurrentRound => Rounds.FirstOrDefault(r => r.IsCurrent);
    public Player? Host => Players.FirstOrDefault(p => p.IsHost);
    public ICollection<Player> Players { get; set; }
    public ICollection<Round> Rounds { get; set; }
}
