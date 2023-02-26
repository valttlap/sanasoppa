namespace Sanasoppa.API.Entities
{
    public class Round
    {
        public Round(Game game, string word) 
        { 
            GameId = game.Id;
            Game = game;
            Word = word;
            Explanations = new List<Explanation>();
        }
        public int Id { get; set; }
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string Word { get; set; }
        public List<Explanation> Explanations { get; set; }
    }
}
