using Sanasoppa.Domain.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IPlayerRepository
{
    /// <summary>
    /// GetPlayerAsync returns a Task<Player?> that represents the asynchronous operation of getting
    /// a Player with the given id.
    /// </summary>
    /// <param name="id">The id of the player you want to get.</param>
    Task<Player?> GetPlayerAsync(int id);
    /// <summary>
    /// > Get the player associated with the given connection id
    /// </summary>
    /// <param name="connectionId">The connection id of the player you want to get.</param>
    Task<Player?> GetPlayerByConnIdAsync(string connectionId);
    Task<IEnumerable<Player>> GetPlayersNotInGameAsync();
    /// <summary>
    /// > Get a player by their username
    /// </summary>
    /// <param name="username">The username of the player you want to get.</param>
    /// <summary>
    /// > Get a player by their username
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
    ///
    /// The function returns a `Task<Game?>` which means that it returns a `Task` that will
    /// eventually return a `Game` or `null`
    /// </summary>
    /// <param name="Player">The player you want to get the game of.</param>
    Task<Game?> GetPlayerGameAsync(Player player);
    /// <summary>
    /// > Adds a player to the game
    /// </summary>
    /// <param name="Player">The player object that you want to add to the game.</param>
    Task GivePointsAsync(int playerId, int points);
    void GivePoints(Player player, int points);
    void AddPlayer(Player player);
    /// <summary>
    /// > This function is called every frame and is used to update the player's position and other
    /// variables
    /// </summary>
    /// <param name="Player">The player that the event is being called for.</param>
    void Update(Player player);
}
