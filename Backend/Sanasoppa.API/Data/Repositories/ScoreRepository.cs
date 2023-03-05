using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Data.Repositories
{
    public class ScoreRepository : IScoreRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ScoreRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void AddScore(Score score)
        {
            _context.Scores.Add(score);
        }

        public async Task<IEnumerable<ScoreDto>> GetTotalScoreAsync(Game game)
        {
            // Query the scores for the game and group them by player
            var playerScores = await _context.Scores
                .Where(s => s.Round.GameId == game.Id)
                .GroupBy(s => s.Player)
                .ToListAsync();

            // Calculate the total score for each player and map to ScoreDto
            var scoreDtos = playerScores.Select(ps => new ScoreDto
            {
                PlayerName = ps.Key.Username,
                TotalPoints = ps.Sum(s => s.Points)
            }).ToList();

            return scoreDtos;
        }
        public void Update(Score score)
        {
             _context.Entry(score).State = EntityState.Modified;
        }
    }
}