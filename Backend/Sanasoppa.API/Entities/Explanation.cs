namespace Sanasoppa.API.Entities
{
    public class Explanation
    {
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public bool IsRight { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; } = default!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;
    }
}
