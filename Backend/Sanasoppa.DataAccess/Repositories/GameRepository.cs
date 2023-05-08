using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.Applicaton.DTOs;
using Sanasoppa.Domain.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;
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

    public void AddPlayerToGame(Game game, Player player)
    {
        if (game.Players.Any(p => p.Id == player.Id)) return;
        game.Players.Add(player);
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

    public async Task<bool> GameExistsAsync(int id)
    {
        return await _context.Games.FindAsync(id) != null;
    }

    public async Task<bool> GameExistsAsync(string gameName)
    {
        return await _context.Games.AnyAsync(g => g.Name == gameName);
    }

    public async Task<Player?> GetDasherAsync(Game game)
    {
        var round = await _context.Rounds
            .Include(r => r.Dasher)
            .Where(r => r.IsCurrent && r.GameId == game.Id)
            .FirstOrDefaultAsync();
        return round?.Dasher;
    }

    public async Task<Game?> GetGameAsync(int id)
    {
        return await _context.Games.FindAsync(id);
    }

    public async Task<Game?> GetGameAsync(string gameName)
    {
        return await _context.Games.Where(g => g.Name == gameName).SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<Game?>> GetGamesAsync()
    {
        return await _context.Games.ToListAsync();
    }

    public async Task<Game?> GetGameWithPlayersAsync(int id)
    {
        return await _context.Games
            .Include(g => g.Players)
            .Where(g => g.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Game?> GetGameWithPlayersAsync(string gameName)
    {
        return await _context.Games
            .Include(g => g.Players)
            .Where(g => g.Name == gameName)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<GameDto?>> GetNotStartedGamesAsync()
    {
        return await _context.Games
            .Include(g => g.Players)
            .Where(g => g.GameState == GameState.NotStarted)
            .ProjectTo<GameDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<Game?> GetWholeGameAsync(int id)
    {
        return await _context.Games
            .Include(g => g.Players)
            .Include(g => g.Rounds)
            .Where(g => g.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<Game?> GetWholeGameAsync(string gameName)
    {
        return await _context.Games
            .Include(g => g.Players)
            .Include(g => g.Rounds)
            .Where(g => g.Name == gameName)
            .SingleOrDefaultAsync();
    }

    public bool HasGameEnded(Game game)
    {
        return game.Players.Any(p => p.TotalPoints >= 20);
    }

    public void RemoveGame(Game game)
    {
        _context.Games.Remove(game);
    }

    public void RemovePlayerFromGame(Game game, string playerName)
    {
        var playerToRemove = game.Players.FirstOrDefault(p => p.Username == playerName);
        if (playerToRemove == null)
        {
            throw new InvalidOperationException($"Player {playerName} is not in the game");
        }
        game.Players.Remove(playerToRemove);
        Update(game);
    }

    public void StartGame(Game game)
    {
        game.GameState = GameState.WaitingDasher;
        Update(game);
    }

    public void Update(Game game)
    {
        _context.Entry(game).State = EntityState.Modified;
    }
}

