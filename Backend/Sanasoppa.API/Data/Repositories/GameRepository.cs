using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public GameRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Game?> GetGameByNameAsync(string name)
        {
            return await _context.Games
                .SingleOrDefaultAsync(x => x.Name == name);
        }

        public async Task<Game?> GetGameByIdAsync(int id)
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

        public async Task<IEnumerable<Player>> GetPlayersAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);
            if (game == null) throw new ArgumentException("Game not found", nameof(gameId));
            return game.Players.ToList();
        }

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
        }

        public async Task<Player> GetDasherAsync(int gameId)
        {
            var game = await _context.Games.FindAsync(gameId);

            if (game == null)
            {
                throw new ArgumentException("Game not found", nameof(gameId));
            }
            
            var dasher = game.Players.SingleOrDefault(p => p.IsDasher ?? false);
            
            if (dasher == null)
            {
                throw new Exception("No dasher found for game");
            }

            return dasher;
        }

        public Player GetDasherAsync(Game game)
        {
            var dasher = game.Players.SingleOrDefault(p => p.IsDasher ?? false);

            if (dasher == null)
            {
                throw new Exception("No dasher found for game");
            }

            return dasher;
        }

        public async Task<IEnumerable<GameDto>> GetNotStartedGamesAsync()
        {
            return await _context.Games
                .Where(g => !g.HasStarted)
                .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}
