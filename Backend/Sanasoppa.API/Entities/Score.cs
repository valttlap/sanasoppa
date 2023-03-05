namespace Sanasoppa.API.Entities
{
    public class Score
    {
        public int Id { get; set; }
        public int Points { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;
        public int RoundId { get; set; }
        public Round Round { get; set; } = default!;
    }
}