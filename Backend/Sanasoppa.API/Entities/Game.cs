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
    public int? CurrentRoundId { get; set; }
    public GameState GameState { get; set; }
    public int HostId { get; set; }
    
    public Round? CurrentRound { get; set; }
    public ICollection<Player> Players { get; set; }
    public ICollection<Round> Rounds { get; set; }
    public Player Host { get; set; } = default!;
}
