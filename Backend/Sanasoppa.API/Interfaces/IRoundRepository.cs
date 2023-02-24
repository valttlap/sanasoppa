using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IRoundRepository
    {
        Task AddExplanationAsync(int roundId, Explanation explanation);
        void AddExplanation(Round round, Explanation explanation);
        void AddRound(Round round);
        void Update(Round round);
    }
}