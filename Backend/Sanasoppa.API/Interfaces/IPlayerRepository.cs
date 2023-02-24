using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IPlayerRepository
    {
        Task<Player> GetPlayerAsync(int id);
        Task<Player> GetPlayerByConnIdAsync(string connectionId);
        Task<Game> GetPlayerGameAsync(string connId);
        Task<Game> GetPlayerGameAsync(Player player);
        void Update(Player player);
    }
}
