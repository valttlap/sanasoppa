namespace Rappakalja.API.Entities
{
    public class Game
    {
        public Game()
        { 
            Players = new List<Player>();    
        }
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Player> Players { get; set; } = default!;
        public int? CurrentRoundId { get; set; }
        public Round? CurrentRound { get; set; }
    }
}
