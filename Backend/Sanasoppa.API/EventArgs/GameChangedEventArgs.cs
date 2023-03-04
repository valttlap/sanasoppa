using Sanasoppa.API.Entities;

namespace Sanasoppa.API.EventArgs
{
    public class GameListChangedEventArgs : System.EventArgs
    {
        public IEnumerable<Game> Games { get; }

        public GameListChangedEventArgs(IEnumerable<Game> games)
        {
            Games = games;
        }
    }
}
