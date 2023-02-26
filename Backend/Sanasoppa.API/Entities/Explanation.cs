namespace Sanasoppa.API.Entities
{
    public class Explanation
    {
        public Explanation(string explanation, bool isRight, Round round, Player player) 
        {
            Text = explanation;
            IsRight = isRight;
            Round = round;
            Player = player;
            RoundId = round.Id;
            PlayerId = player.Id;
        }
        public int Id { get; set; }
        public string Text { get; set; } = default!;
        public bool IsRight { get; set; }
        public int RoundId { get; set; }
        public Round Round { get; set; } = default!;
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!;
    }
}
