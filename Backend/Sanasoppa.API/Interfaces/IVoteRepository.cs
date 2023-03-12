using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IVoteRepository
    {
        Task<IEnumerable<Vote>> GetRoundVotesAsync(int roundId);
        Task<IEnumerable<Vote>> GetRoundVotesAsync(Round round);

        void AddVote(Vote vote);
        void UpdateVote(Vote vote);
    }
}