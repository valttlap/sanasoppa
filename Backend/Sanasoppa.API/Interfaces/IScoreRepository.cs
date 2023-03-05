using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Interfaces
{
    public interface IScoreRepository
    {
        public Task<IEnumerable<ScoreDto>> GetTotalScoreAsync(Game game);
        public void AddScore(Score score);
        public void Update(Score score);
    }
}