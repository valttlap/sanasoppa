using Microsoft.EntityFrameworkCore;
using Sanasoppa.Domain.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories;
public class VoteRepository : IVoteRepository
{
    private readonly DataContext _context;

    public VoteRepository(DataContext context)
    {
        _context = context;
    }

    public void AddVote(Vote vote)
    {
        _context.Votes.Add(vote);
    }

    public async Task<IEnumerable<Vote>> GetRoundVotesAsync(int roundId)
    {
        return await _context.Votes
            .Where(v => v.RoundId == roundId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vote>> GetRoundVotesAsync(Round round)
    {
        return await GetRoundVotesAsync(round.Id);
    }

    public void UpdateVote(Vote vote)
    {
        _context.Entry(vote).State = EntityState.Modified;
    }
}
