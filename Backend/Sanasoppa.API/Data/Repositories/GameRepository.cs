using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;
using System.Numerics;

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

        public void AddGame(Game game)
        {
            _context.Games.Add(game);
        }

        public async Task AddPlayerToGameAsync(string gameName, string playerName)
        {
            var game = await GetGameWithPlayersAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            if (game.Players.Any(p => p.Username == playerName)) return;

            var playerToAdd = await _context.Players.Where(p => p.Username == playerName).SingleOrDefaultAsync();
            if (playerToAdd == null)
            {
                throw new InvalidOperationException($"Player {playerName} doesn't exists");
            }
            game.Players.Add(playerToAdd);
            Update(game);
        }

        public async Task AddPlayerToGameAsync(string gameName, Player player)
        {
            var game = await GetGameWithPlayersAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            if (game.Players.Any(p => p.Username == player.Username)) return;

            game.Players.Add(player);
            Update(game);
        }

        public async Task AddPlayerToGameAsync(int id, string playerName)
        {
            var game = await GetGameWithPlayersAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            if (game.Players.Any(p => p.Username == playerName)) return;

            var playerToAdd = await _context.Players.Where(p => p.Username == playerName).SingleOrDefaultAsync();
            if (playerToAdd == null)
            {
                throw new InvalidOperationException($"Player {playerName} doesn't exists");
            }
            game.Players.Add(playerToAdd);
            Update(game);
        }

        public async Task AddPlayerToGameAsync(Game game, string playerName)
        {
            if (game.Players.Any(p => p.Username == playerName)) return;

            var playerToAdd = await _context.Players.Where(p => p.Username == playerName).SingleOrDefaultAsync();
            if (playerToAdd == null)
            {
                throw new InvalidOperationException($"Player {playerName} doesn't exists");
            }
            game.Players.Add(playerToAdd);
            Update(game);
        }

        public async Task AddPlayerToGameAsync(int id, Player player)
        {
            var game = await GetGameWithPlayersAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            if (game.Players.Any(p => p.Id == player.Id)) return;

            game.Players.Add(player);
            Update(game);
        }

        public void AddPlayerToGameAsync(Game game, Player player)
        {
            if (game.Players.Any(p => p.Id == player.Id)) return;

            game.Players.Add(player);
            Update(game);
        }

        public async Task<bool> GameExistsAsync(string gameName)
        {
            return await _context.Games.AnyAsync(g => g.Name == gameName);
        }

        public async Task<bool> GameExistsAsync(int id)
        {
            return await _context.Games.FindAsync(id) != null;
        }

        public async Task<Player?> GetDasherAsync(string gameName)
        {
            var game = await GetGameWithPlayersAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            return game.Players.SingleOrDefault(p => p.IsDasher == true);
        }

        public async Task<Player?> GetDasherAsync(int id)
        {
            var game = await GetGameWithPlayersAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            return game.Players.SingleOrDefault(p => p.IsDasher == true);
        }

        public Player? GetDasherAsync(Game game)
        {
            return game.Players.SingleOrDefault(p => p.IsDasher == true);
        }

        public async Task<Game?> GetGameAsync(string gameName)
        {
            return await _context.Games.Where(g => g.Name == gameName).SingleOrDefaultAsync();
        }

        public async Task<Game?> GetGameAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<IEnumerable<Game?>> GetGamesAsync()
        {
            return await _context.Games.ToListAsync();
        }

        public async Task<Game?> GetGameWithPlayersAsync(string gameName)
        {
            return await _context.Games
                .Include(g => g.Players)
                .Where(g => g.Name == gameName)
                .SingleOrDefaultAsync();
        }

        public async Task<Game?> GetGameWithPlayersAsync(int id)
        {
            return await _context.Games
                .Include(g => g.Players)
                .Where(g => g.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<GameDto?>> GetNotStartedGamesAsync()
        {
            return await _context.Games
                .Include(g => g.Players)
                .Where(g => !g.HasStarted)
                .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(string gameName)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Name == gameName)
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(int id)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Id == id)
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(Game game)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Id == game.Id)
                .ProjectTo<PlayerDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player?>> GetPlayersAsync(string gameName)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Name == gameName)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player?>> GetPlayersAsync(int id)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Id == id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Player?>> GetPlayersAsync(Game game)
        {
            return await _context.Players
                .Include(p => p.Game)
                .Where(p => p.Game != null && p.Game.Id == game.Id)
                .ToListAsync();
        }

        public async Task<Game?> GetWholeGame(int id)
        {
            return await _context.Games
                .Include(g => g.Players)
                .Include(g => g.CurrentRound)
                .Where(g => g.Id == id)
                .SingleOrDefaultAsync();
        }

        public async Task RemoveGameAsync(string gameName)
        {
            var game = await GetGameAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            _context.Games.Remove(game);
        }

        public async Task RemoveGameAsync(int id)
        {
            var game = await GetGameAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            _context.Games.Remove(game);
        }

        public void RemoveGameAsync(Game game)
        {
            _context.Games.Remove(game);
        }

        public async Task RemovePlayerFromGameAsync(string gameName, string playerName)
        {
            var game = await GetGameWithPlayersAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            var playerToRemove = game.Players.FirstOrDefault(p => p.Username == playerName);
            if (playerToRemove == null)
            {
                throw new InvalidOperationException($"Player {playerName} is not in the game");
            }
            game.Players.Remove(playerToRemove);
            Update(game);
        }

        public async Task RemovePlayerFromGameAsync(int id, string playerName)
        {
            var game = await GetGameWithPlayersAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            var playerToRemove = game.Players.FirstOrDefault(p => p.Username == playerName);
            if (playerToRemove == null)
            {
                throw new InvalidOperationException($"Player {playerName} is not in the game");
            }
            game.Players.Remove(playerToRemove);
            Update(game);
        }

        public void RemovePlayerFromGameAsync(Game game, string playerName)
        {
            var playerToRemove = game.Players.FirstOrDefault(p => p.Username == playerName);
            if (playerToRemove == null)
            {
                throw new InvalidOperationException($"Player {playerName} is not in the game");
            }
            game.Players.Remove(playerToRemove);
            Update(game);
        }

        public async Task StartGameAsync(string gameName)
        {
            var game = await GetGameAsync(gameName);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            game.HasStarted = true;
            Update(game);
        }

        public async Task StartGameAsync(int id)
        {
            var game = await GetGameAsync(id);
            if (game == null)
            {
                throw new InvalidOperationException($"The Game {{game}} does not exists");
            }
            game.HasStarted = true;
            Update(game);
        }

        public void StartGameAsync(Game game)
        {
            game.HasStarted = true;
            Update(game);
        }

        public void Update(Game game)
        {
            _context.Entry(game).State = EntityState.Modified;
        }
    }
}
