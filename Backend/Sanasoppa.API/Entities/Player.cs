using System.Diagnostics.CodeAnalysis;

namespace Sanasoppa.API.Entities
{
    public class Player
    {
        public Player()
        {
            Explanations = new List<Explanation>();
        }
        public int Id { get; set; }
        public string ConnectionId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public bool IsOnline { get; set; }
        public bool? IsDasher { get; set; }
        public int? GameId { get; set; }
        public Game? Game { get; set; }
        public List<Explanation> Explanations { get; set; }
    }
}
