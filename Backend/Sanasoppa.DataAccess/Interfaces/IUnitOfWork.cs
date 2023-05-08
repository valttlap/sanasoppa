using Sanasoppa.API.EventArgs;

namespace Sanasoppa.API.Interfaces;

public interface IUnitOfWork
{
    IGameRepository GameRepository { get; }
    IPlayerRepository PlayerRepository { get; }
    IRoundRepository RoundRepository { get; }
    IExplanationRepository ExplanationRepository { get; }
    IVoteRepository VoteRepository { get; }

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <returns>True if the save was successful, false otherwise.</returns>
    Task<bool> Complete();

    /// <summary>
    /// Checks if the context has any changes.
    /// </summary>
    /// <returns>True if the context has changes, false otherwise.</returns>
    bool HasChanges();

    /// <summary>
    /// Event that is fired when the list of games changes.
    /// </summary>
    event EventHandler<GameListChangedEventArgs>? GameListChanged;

    /// <summary>
    /// Subscribes a handler to the GameListChanged event.
    /// </summary>
    /// <param name="handler">The handler to subscribe.</param>
    void SubscribeToGameListChangedEvent(EventHandler<GameListChangedEventArgs> handler);
}
