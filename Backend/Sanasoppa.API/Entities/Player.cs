namespace Sanasoppa.API.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string ConnectionId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public bool IsDasher { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = default!;
        public List<Explanation> Explanations { get; set; } = new();
    }
}
