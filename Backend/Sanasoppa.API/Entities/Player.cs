using System.Diagnostics.CodeAnalysis;

namespace Sanasoppa.API.Entities
{
    public class Player
    {
        public Player(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
            Explanations = new List<Explanation>();
        }
        public int Id { get; set; }
        public string ConnectionId { get; set; }
        public string Username { get; set; }
        public bool? IsDasher { get; set; }
        public int? GameId { get; set; }
        public Game? Game { get; set; }
        public List<Explanation> Explanations { get; set; }
    }
}
