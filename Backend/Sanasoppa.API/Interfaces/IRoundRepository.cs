using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IRoundRepository
    {
        /// <summary>
        /// > Adds an explanation to the round
        /// </summary>
        /// <param name="roundId">The id of the round that the explanation is for.</param>
        /// <param name="Explanation">The explanation object that you want to add to the round.</param>
        Task AddExplanationAsync(int roundId, Explanation explanation);
        /// <summary>
        /// > Adds an explanation to the round
        /// </summary>
        /// <param name="Round">The round that the explanation is for.</param>
        /// <param name="Explanation">The explanation object that you want to add to the round.</param>
        void AddExplanation(Round round, Explanation explanation);
        Task<Round?> GetRoundWithExplanationsAsync(int roundId);
        /// <summary>
        /// > Adds a round to the game
        /// </summary>
        /// <param name="Round">The round object that you want to add to the database.</param>
        void AddRound(Round round);
        /// <summary>
        /// > Update the game state with the given round
        /// </summary>
        /// <param name="Round">The round object that you want to update.</param>
        void Update(Round round);
    }
}