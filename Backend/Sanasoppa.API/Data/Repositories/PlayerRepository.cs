using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly DataContext _context;

        public PlayerRepository(DataContext context)
        {
            _context = context;
        }

        public void AddPlayer(Player player)
        {
            _context.Players.Add(player);
        }

        public async Task<Player?> GetPlayerAsync(int id)
        {
            return await _context.Players.FindAsync(id);
        }

        public async Task<Player?> GetPlayerByConnIdAsync(string connectionId)
        {
            return await _context.Players
                .SingleOrDefaultAsync(x => x.ConnectionId == connectionId);
        }

        public async Task<Player?> GetPlayerByUsernameAsync(string username)
        {
            return await _context.Players.SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<Game?> GetPlayerGameAsync(string connId)
        {
            var game = await _context.Players
                .Where(p => p.ConnectionId == connId)
                .Select(p => p.Game)
                .FirstOrDefaultAsync();
            return game;
        }

        public async Task<Game?> GetPlayerGameAsync(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            return await _context.Games.Include(g => g.CurrentRound).Where(g => g.Id == player.GameId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Player>> GetPlayersNotInGameAsync()
        {
            return await _context.Players
                .Where(p => p.GameId == null)
                .ToListAsync();
        }

        public void Update(Player player)
        {
            _context.Entry(player).State = EntityState.Modified;
        }
    }
}
