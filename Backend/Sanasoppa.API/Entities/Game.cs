namespace Sanasoppa.API.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public bool GameStarted { get; set; }
        public List<Player> Players { get; set; } = new();
        public Round CurrentRound { get; set; }
    }
}
