using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class ExplanationRepository : IExplanationRepository
    {
        private readonly DataContext _context;

        public ExplanationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Explanation?> GetExplanationAsync(int playerId, int roundId)
        {
            return await _context.Explanations
                .Include(e => e.Player)
                .Include(e => e.Round)
                .FirstOrDefaultAsync(e => e.Player.Id == playerId && e.Round.Id == roundId);
        }

        public async Task RemoveExplanationAsync(int playerId, int roundId)
        {
            var explanation = await GetExplanationAsync(playerId, roundId);
            if (explanation == null)
            {
                throw new ArgumentException("The explanation does not exist.", nameof(playerId));
            }
            _context.Explanations.Remove(explanation);
        }

        public void AddExplanation(Explanation explanation)
        {
            _context.Explanations.Add(explanation);
        }

        public void Update(Explanation explanation)
        {
            _context.Entry(explanation).State = EntityState.Modified;
        }

        public async Task<Explanation?> GetExplanationAsync(int explanationId)
        {
            return await _context.Explanations
                .Include(e => e.Player)
                .Include(e => e.Round)
                .FirstOrDefaultAsync(e => e.Id == explanationId);
        }

        public async Task<IEnumerable<Explanation>> GetRoundExplanationsWithVotesAsync(int roundId)
        {
            return await _context.Explanations
                .Include(e => e.Player)
                .Include(e => e.Votes)
                .Where(e => e.RoundId == roundId)
                .ToListAsync();
        }
    }
}