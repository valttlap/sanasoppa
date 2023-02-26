namespace Sanasoppa.API.Entities
{
    public class Game
    {
        public Game(string name, bool hasStarted = false) 
        {
            Name = name;
            HasStarted = hasStarted;
            Players = new List<Player>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasStarted { get; set; }
        public ICollection<Player> Players { get; set; }
        public int? CurrentRoundId { get; set; }
        public Round? CurrentRound { get; set; }
    }
}
