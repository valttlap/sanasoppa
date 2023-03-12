namespace Sanasoppa.API.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int PlayerId { get; set; }
        public int ExplanationId { get; set; }

        public Round Round { get; set; } = default!;
        public Player Player { get; set; } = default!;
        public Explanation Explanation { get; set; } = default!;
    }
}