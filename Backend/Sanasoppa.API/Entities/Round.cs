namespace Sanasoppa.API.Entities
{
    public class Round
    {
        public Round() 
        { 
            Explanations = new List<Explanation>();
            Scores = new List<Score>();
        }
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; } = default!;
        public string Word { get; set; } = default!;
        public List<Explanation> Explanations { get; set; }
        public List<Score> Scores { get; set; }
    }
}
