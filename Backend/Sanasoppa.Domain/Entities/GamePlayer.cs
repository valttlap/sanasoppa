namespace Sanasoppa.Domain.Entities;

public class GamePlayer
{
    public int GameId { get; private set; }
    public int PlayerId { get; private set; }
    public byte Order { get; private set; }

    public Game Game { get; private set; }
    public Player Player { get; private set; }

    private GamePlayer() { }

    internal GamePlayer(Game game, Player player, byte order)
    {
        Game = game;
        Player = player;
        Order = order;
    }
}
