namespace Sanasoppa.Domain.Entities;

public class Game
{
    public int GameId { get; private set; }
    public string Name { get; private set; }
    public GameState GameState { get; private set; }

    private HashSet<Round>? _rounds;
    private HashSet<GamePlayer>? _playersLink;

    public IEnumerable<Round>? Rounds => _rounds?.ToList();
    public IEnumerable<GamePlayer>? PlayersLink => _playersLink?.ToList();
    public bool HasStarted => GameState != GameState.NotStarted;

    private Game() { }

    public Game(string name, GameState gameState, ICollection<Player> players)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
        GameState = gameState;
        _rounds = new HashSet<Round>();

        if (players is null || players.SingleOrDefault(p => p.IsHost) is null)
            throw new ArgumentException("You must have at least one player for game who is host", nameof(players));
        byte order = 0;
        _playersLink = new HashSet<GamePlayer>(players.Select(p => new GamePlayer(this, p, order++)));
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    public void ChangeGameState(GameState gameState)
    {
        GameState = gameState;
    }

    public void AddRound(int )
}





public class GameOld
{
    public GameOld()
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
