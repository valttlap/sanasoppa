namespace Rappakalja.API.Entities
{
    public class Round
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = default!;
        public String Word { get; set; } = default!;
        public List<Explanation> Explanations = new();
    }
}
