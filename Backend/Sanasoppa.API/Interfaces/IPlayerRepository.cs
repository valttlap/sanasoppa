// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IPlayerRepository
{
    /// <summary>
    /// GetPlayerAsync returns a Player with id
    /// </summary>
    /// <param name="id">The id of the player you want to get.</param>
    /// <returns> Player if found, else null</returns>
    Task<Player?> GetPlayerAsync(int id);
    /// <summary>
    /// > Get the player associated with the given connection id
    /// </summary>
    /// <param name="connectionId">The connection id of the player you want to get.</param>
    Task<Player?> GetPlayerByConnIdAsync(string connectionId);
    /// <summary>
    /// Get all players that are not in a game
    /// </summary>
    /// <returns>A collection of all players that are not in a game.</returns>
    Task<IEnumerable<Player>> GetPlayersNotInGameAsync();
    /// <summary>
    /// Get a player by their username
    /// </summary>
    /// <param name="username">The username of the player you want to get.</param>
    Task<Player?> GetPlayerByUsernameAsync(string username);
    /// <summary>
    /// > Get the game that the player with the given connection ID is in
    /// </summary>
    /// <param name="connId">The connection id of the player.</param>
    Task<Game?> GetPlayerGameAsync(string connId);
    /// <summary>
    /// "Get the game that the player is in, if any."
    /// </summary>
    /// <param name="player">The player you want to get the game of.</param>
    /// <returns>The game that the player is in, if any.</returns>
    Task<Game?> GetPlayerGameAsync(Player player);
    /// <summary>
    /// Give points to a player
    /// </summary>
    /// <param name="playerId">The id of the player you want to give points to.</param>
    /// <param name="points">The amount of points you want to give.</param>
    /// <returns></returns>
    Task GivePointsAsync(int playerId, int points);
    void GivePoints(Player player, int points);
    void AddPlayer(Player player);
    void Update(Player player);
}
