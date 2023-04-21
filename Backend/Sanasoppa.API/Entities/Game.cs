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
    public bool HasStarted { get; set; }
    public GameState GameState { get; set; }
    public Round? CurrentRound => Rounds.FirstOrDefault(r => r.IsCurrent);
    public Player? Host => Players.FirstOrDefault(p => p.IsHost);
    public ICollection<Player> Players { get; set; }
    public ICollection<Round> Rounds { get; set; }
}
