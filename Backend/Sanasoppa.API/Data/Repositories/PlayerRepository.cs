// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;

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
        return await _context.Players.FindAsync(id).ConfigureAwait(false);
    }

    public async Task<Player?> GetPlayerByConnIdAsync(string connectionId)
    {
        return await _context.Players
            .SingleOrDefaultAsync(x => x.ConnectionId == connectionId).ConfigureAwait(false);
    }

    public async Task<Player?> GetPlayerByUsernameAsync(string username)
    {
        return await _context.Players.SingleOrDefaultAsync(x => x.Username == username).ConfigureAwait(false);
    }

    public async Task<Game?> GetPlayerGameAsync(string connId)
    {
        var game = await _context.Players
            .Where(p => p.ConnectionId == connId)
            .Select(p => p.Game)
            .FirstOrDefaultAsync().ConfigureAwait(false);
        return game;
    }

    public async Task<Game?> GetPlayerGameAsync(Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        return await _context.Games.Include(g => g.Rounds).Where(g => g.Id == player.GameId).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Player>> GetPlayersNotInGameAsync()
    {
        return await _context.Players
            .Where(p => p.GameId == null)
            .ToListAsync().ConfigureAwait(false);
    }

    public void GivePoints(Player player, int points)
    {
        player.TotalPoints += points;
        Update(player);
    }

    public async Task GivePointsAsync(int playerId, int points)
    {
        var player = await GetPlayerAsync(playerId).ConfigureAwait(false) ?? throw new ArgumentException("The player does not exist.", nameof(playerId));
        GivePoints(player, points);
    }

    public void Update(Player player)
    {
        _context.Entry(player).State = EntityState.Modified;
    }
}
