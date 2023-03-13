using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;
public class RoundRepository : IRoundRepository
{
    private readonly DataContext _context;

    public RoundRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddExplanationAsync(int roundId, Explanation explanation)
    {
        var round = await _context.Rounds.FindAsync(roundId);
        if (round == null)
        {
            throw new ArgumentException("The round does not exist.", nameof(roundId));
        }
        round.Explanations.Add(explanation);
        Update(round);
    }

    public void AddExplanation(Round round, Explanation explanation)
    {
        if (round == null)
        {
            throw new ArgumentNullException(nameof(round));
        }
        round.Explanations.Add(explanation);
        Update(round);
    }

    public void AddRound(Round round)
    {
        _context.Rounds.Add(round);
    }

    public void Update(Round round)
    {
        _context.Entry(round).State = EntityState.Modified;
    }

    public async Task<Round?> GetRoundWithExplanationsAsync(int roundId)
    {
        return await _context.Rounds
            .Include(r => r.Explanations)
            .FirstOrDefaultAsync(r => r.Id == roundId);
    }

}
