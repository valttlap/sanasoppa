using Sanasoppa.Domain.Entities;

namespace Sanasoppa.API.Interfaces;

public interface IExplanationRepository
{
    public Task<Explanation?> GetExplanationAsync(int explanationId);
    public Task<Explanation?> GetExplanationAsync(int playerId, int roundId);
    public Task<IEnumerable<Explanation>> GetRoundExplanationsWithVotesAsync(int roundId);
    public Task RemoveExplanationAsync(int playerId, int roundId);
    public void AddExplanation(Explanation explanation);
    public void Update(Explanation explanation);
}
