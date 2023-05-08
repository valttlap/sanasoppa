namespace Sanasoppa.Domain.Entities;

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
