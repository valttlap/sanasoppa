using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.EventArgs;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public event EventHandler<GameListChangedEventArgs>? GameListChanged;

    public UnitOfWork(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public IGameRepository GameRepository => new GameRepository(_context, _mapper);

    public IPlayerRepository PlayerRepository => new PlayerRepository(_context);

    public IRoundRepository RoundRepository => new RoundRepository(_context);
    public IExplanationRepository ExplanationRepository => new ExplanationRepository(_context);
    public IVoteRepository VoteRepository => new VoteRepository(_context);
    public IRefreshTokenRepository RefreshTokenRepository => new RefreshTokenRepository(_context);



    public async Task<bool> Complete()
    {
        var hasGameChanges = HasGameChanged();
        var saved = await _context.SaveChangesAsync() > 0;
        if (saved && hasGameChanges)
        {
            OnGameListChanged(new GameListChangedEventArgs(Enumerable.Empty<Game>()));
        }
        return saved;
    }

    public bool HasChanges()
    {
        return _context.ChangeTracker.HasChanges();
    }

    protected virtual void OnGameListChanged(GameListChangedEventArgs e)
    {
        GameListChanged?.Invoke(this, e);
    }

    public void SubscribeToGameListChangedEvent(EventHandler<GameListChangedEventArgs> handler)
    {
        GameListChanged += handler;
    }

    /// <summary>
    /// Checks if there has been any changes to the game or its players, such as a new game being added,
    /// a player being added to a game or a game being deleted. Also checks if a game that hasn't started
    /// has been removed, if a player has been removed from a game that hasn't started, or if a game has started.
    /// </summary>
    /// <returns>True if any changes have been made to the game or its players, false otherwise</returns>
    private bool HasGameChanged()
    {
        return _context.ChangeTracker.Entries<Game>()
            .Any(e => e.State == EntityState.Added) ||
            _context.ChangeTracker.Entries<Game>()
            .Any(e => e.State == EntityState.Deleted && !e.Entity.HasStarted) ||
            _context.ChangeTracker.Entries<Player>()
            .Any(e => e.State == EntityState.Added && e.Entity.Game != null) ||
            _context.ChangeTracker.Entries<Player>()
            .Any(e => e.State == EntityState.Deleted && e.Entity.Game != null && !e.Entity.Game.HasStarted) ||
            _context.ChangeTracker.Entries<Game>()
            .Any(e => e.State == EntityState.Modified && e.Entity.HasStarted);
    }

}
