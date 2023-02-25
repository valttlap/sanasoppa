using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetGamesAsync();
        Task<Game> GetByIdAsync(int id);
        Task<Game> GetByConnectionIdAsync(int connectionId);
        Task<IEnumerable<Player>> GetPlayersAsync(int gameId);
        Task<Player> GetDasherAsync(int gameId);
        Task<Player> GetDasherAsync(Game game);
        Task<int> GetNextConnectionId();
        void AddGame(Game game);
        void Update(Game game);
    }
}
