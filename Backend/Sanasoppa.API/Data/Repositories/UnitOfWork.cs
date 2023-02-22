using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
        }
        public IGameRepository GameRepository => new GameRepository(_context);

        public IPlayerRepository PlayerRepository => new PlayerRepository(_context);

        public async Task<bool> Complete()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}