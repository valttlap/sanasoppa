using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Game?> GetByConnectionIdAsync(int connectionId)
        {
            return await _context.Games
                .SingleOrDefaultAsync(x => x.ConnectionId == connectionId);
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<IEnumerable<Game>> GetGamesAsync()
        {
            return await _context.Games
                .ToListAsync();
        }

        public void Update(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Player?>> GetPlayersAsync(int gameId)
        {
            return await _context.Players.Where(p => p.GameId == gameId).ToListAsync();
        }

        public async Task<int> GetNextConnectionId()
        {
            int maxId = await _context.Games.MaxAsync(g => (int?)g.ConnectionId) ?? 0;
            return maxId + 1;
        }

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
        }

        public async Task<Player> GetDasherAsync(int gameId)
        {
            var dasher = await _context.Players
                .SingleOrDefaultAsync(p => p.GameId == gameId && p.IsDasher);
            
            if (dasher == null)
            {
                throw new Exception("No dasher found for game");
            }

            return dasher;
        }

        public async Task<Player> GetDasherAsync(Game game)
        {
            var dasher = await _context.Players
                .SingleOrDefaultAsync(p => p.GameId == game.Id && p.IsDasher);

            if (dasher == null)
            {
                throw new Exception("No dasher found for game");
            }

            return dasher;
        }

    }
}
