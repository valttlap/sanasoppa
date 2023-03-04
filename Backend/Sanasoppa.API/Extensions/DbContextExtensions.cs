using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Extensions
{
    public static class DbContextExtensions
    {
        public static async Task SeedDataAsync(this DataContext context)
        {
            var gameStates = new List<GameState>
            {
                new GameState { Id = 1, Name = "Not started" },
                new GameState { Id = 2, Name = "Waiting dasher" },
                new GameState { Id = 3, Name = "Giving explanations" },
                new GameState { Id = 4, Name = "Dasher valuing explanations" },
                new GameState { Id = 5, Name = "Voting explanations" },
                new GameState { Id = 6, Name = "Calculating points" },
                new GameState { Id = 7, Name = "Game ended" }

            };

            foreach (var gameState in gameStates)
            {
                if (!await context.GameStates.AnyAsync(g => g.Name == gameState.Name))
                {
                    context.GameStates.Add(gameState);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
