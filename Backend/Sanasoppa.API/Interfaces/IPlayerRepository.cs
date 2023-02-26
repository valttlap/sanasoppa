using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player?> GetPlayerAsync(int id);
        Task<Player?> GetPlayerByConnIdAsync(string connectionId);
        Task<IEnumerable<Player>> GetPlayersNotInGameAsync();
        Task<Player?> GetPlayerByUsernameAsync(string username);
        Task<Game?> GetPlayerGameAsync(string connId);
        Task<Game?> GetPlayerGameAsync(Player player);
        void AddPlayer(Player player);
        void Update(Player player);
    }
}
