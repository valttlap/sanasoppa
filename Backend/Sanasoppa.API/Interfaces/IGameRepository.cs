using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.EventArgs;

namespace Sanasoppa.API.Interfaces
{
    public interface IGameRepository
    {
        /// <summary>
        /// Get all games.
        /// </summary>
        /// <returns>Returns a collection of all games.</returns>
        Task<IEnumerable<Game?>> GetGamesAsync();

        /// <summary>
        /// Get a game by its id.
        /// </summary>
        /// <param name="id">The id of the game to retrieve.</param>
        /// <returns>Returns the game with the id, or null if no such game exists.</returns>
        Task<Game?> GetGameAsync(int id);

        /// <summary>
        /// Get a game by its name.
        /// </summary>
        /// <param name="gameName">The name of the game to retrieve.</param>
        /// <returns>Returns the game with the specified name, or null if no such game exists.</returns>
        Task<Game?> GetGameAsync(string gameName);

        /// <summary>
        /// Get a game by its id with all its players.
        /// </summary>
        /// <param name="id">The id of the game to retrieve.</param>
        /// <returns>Returns the game with the id and its players, or null if no such game exists.</returns>
        Task<Game?> GetGameWithPlayersAsync(int id);

        /// <summary>
        /// Get a game by its name with all its players.
        /// </summary>
        /// <param name="gameName">The name of the game to re
        /// <returns>Returns the game with the specified name
        Task<Game?> GetGameWithPlayersAsync(string gameName);

        /// <summary>
        /// Get all games that have not started yet.
        /// </summary>
        /// <returns>Returns a collection of all games that have not started yet.</returns>
        Task<IEnumerable<GameDto?>> GetNotStartedGamesAsync();

        /// <summary>
        /// Get all players in a game.
        /// </summary>
        /// <param name="id">The id of the game to retrieve the players for.</param>
        /// <returns>Returns a collection of all players in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<Player?>> GetPlayersAsync(int id);

        /// <summary>
        /// Get all players in a game.
        /// </summary>
        /// <param name="gameName">The name of the game to retrieve the players for.</param>
        /// <returns>Returns a collection of all players in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<Player?>> GetPlayersAsync(string gameName);

        /// <summary>
        /// Get all players in a game.
        /// </summary>
        /// <param name="game">The game to retrieve the players for.</param>
        /// <returns>Returns a collection of all players in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<Player?>> GetPlayersAsync(Game game);

        /// <summary>
        /// Get all player DTOs in a game.
        /// </summary>
        /// <param name="id">The id of the game to retrieve the player DTOs for.</param>
        /// <returns>Returns a collection of all player DTOs in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(int id);

        /// <summary>
        /// Get all player DTOs in a game.
        /// </summary>
        /// <param name="gameName">The name of the game to retrieve the player DTOs for.</param>
        /// <returns>Returns a collection of all player DTOs in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(string gameName);

        /// <summary>
        /// Get all player DTOs in a game.
        /// </summary>
        /// <param name="game">The game to retrieve the player DTOs for.</param>
        /// <returns>Returns a collection of all player DTOs in the specified game, or an empty collection if the game does not exist or has no players.</returns>
        Task<IEnumerable<PlayerDto?>> GetPlayerDtosAsync(Game game);

        /// <summary>
        /// Get the dasher in a game.
        /// </summary>
        /// <param name="gameName">The name of the game to retrieve the dasher for.</param>
        /// <returns>Returns the dasher in the specified game, or null if the game does not exist or has no dasher.</returns>
        Task<Player?> GetDasherAsync(string gameName);

        /// <summary>
        /// Get the dasher in a game.
        /// </summary>
        /// <param name="id">The id of the game to retrieve the dasher for.</param>
        /// <returns>Returns the dasher in the specified game, or null if the game does not exist or has no dasher.</returns>
        Task<Player?> GetDasherAsync(int id);

        /// <summary>
        /// Get the dasher in a game.
        /// </summary>
        /// <param name="game">The game to retrieve the dasher for.</param>
        /// <returns>Returns the dasher in the specified game, or null if the game does not exist or has no dasher.</returns>
        Player? GetDasherAsync(Game game);


        /// <summary>
        /// Check if a game with the id exists.
        /// </summary>
        /// <param name="id">The id of the game to check.</param>
        /// <returns>Returns true if a game with the id exists, false otherwise.</returns>
        Task<bool> GameExistsAsync(int id);

        /// <summary>
        /// Check if a game with the specified name exists.
        /// </summary>
        /// <param name="gameName">The name of the game to check.</param>
        /// <returns>Returns true if a game with the specified name exists, false otherwise.</returns>
        Task<bool> GameExistsAsync(string gameName);

        Task<Game?> GetWholeGame(int id);

        /// <summary>
        /// Add a new game to the database.
        /// </summary>
        /// <param name="game">The game to add.</param>
        void AddGame(Game game);

        /// <summary>
        /// Update a game in the database.
        /// </summary>
        /// <param name="game">The game to update.</param>
        void Update(Game game);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="gameName">The name of the game to add the player to.</param>
        /// <param name="playerName">The name of the player to add.</param>
        Task AddPlayerToGameAsync(string gameName, string playerName);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="id">The id of the game to add the player to.</param>
        /// <param name="playerName">The name of the player to add.</param>
        Task AddPlayerToGameAsync(int id, string playerName);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="game">The game to add the player to.</param>
        /// <param name="playerName">The name of the player to add.</param>
        Task AddPlayerToGameAsync(Game game, string playerName);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="gameName">The name of the game to add the player to.</param>
        /// <param name="player">The player object to add.</param>
        Task AddPlayerToGameAsync(string gameName, Player player);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="id">The id of the game to add the player to.</param>
        /// <param name="player">The player object to add.</param>
        Task AddPlayerToGameAsync(int id, Player player);

        /// <summary>
        /// Add a player to a game.
        /// </summary>
        /// <param name="game">The game to add the player to.</param>
        /// <param name="player">The player object to add.</param>
        void AddPlayerToGameAsync(Game game, Player player);

        /// <summary>
        /// Remove a player from a game.
        /// </summary>
        /// <param name="gameName">The name of the game to remove the player from.</param>
        /// <param name="playerName">The name of the player to remove.</param>
        Task RemovePlayerFromGameAsync(string gameName, string playerName);

        /// <summary>
        /// Remove a player from a game.
        /// </summary>
        /// <param name="id">The id of the game to remove the player from.</param>
        /// <param name="playerName">The name of the player to remove.</param>
        Task RemovePlayerFromGameAsync(int id, string playerName);

        /// <summary>
        /// Remove a player from a game.
        /// </summary>
        /// <param name="game">The game to remove the player from.</param>
        /// <param name="playerName">The name of the player to remove.</param>
        void RemovePlayerFromGameAsync(Game game, string playerName);

        /// <summary>
        /// Sets the <see cref="Game.HasStarted"/> property to true, indicating that the game has started.
        /// </summary>
        /// <param name="gameName">The name of the game to start.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartGameAsync(string gameName);

        /// <summary>
        /// Sets the <see cref="Game.HasStarted"/> property to true, indicating that the game has started.
        /// </summary>
        /// <param name="id">The id of the game to start.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task StartGameAsync(int id);

        /// <summary>
        /// Sets the <see cref="Game.HasStarted"/> property to true, indicating that the game has started.
        /// </summary>
        /// <param name="id">The game to start.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        void StartGameAsync(Game game);

        /// <summary>
        /// Removes the game with the specified name from the data store.
        /// </summary>
        /// <param name="gameName">The name of the game to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveGameAsync(string gameName);

        /// <summary>
        /// Removes the game with the id from the data store.
        /// </summary>
        /// <param name="id">The name of the game to remove.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveGameAsync(int id);

        /// <summary>
        /// Removes the game from the data store.
        /// </summary>
        /// <param name="game">The game to remove.</param>
        void RemoveGameAsync(Game game);

    }
}
