using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetGamesAsync();
        Task<IEnumerable<GameDto>> GetNotStartedGamesAsync();
        Task<Game?> GetGameByIdAsync(int id);
        Task<Game?> GetGameByNameAsync(string name);
        Task<IEnumerable<Player>> GetPlayersAsync(int gameId);
        Task<Player> GetDasherAsync(int gameId);
        Player GetDasherAsync(Game game);
        void AddGame(Game game);
        void Update(Game game);
    }
}
