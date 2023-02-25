using AutoMapper;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IGameRepository GameRepository => new GameRepository(_context);

        public IPlayerRepository PlayerRepository => new PlayerRepository(_context);

        public IRoundRepository RoundRepository => new RoundRepository(_context);

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