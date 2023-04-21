using Sanasoppa.API.Entities;
using Sanasoppa.API.Data;
using Sanasoppa.API.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Sanasoppa.API.Tests.Repositories;

[TestFixture]
public class PlayerRepositoryTests
{
    private DbContextOptions<DataContext>? _options;

    private Player? _player1;
    private Player? _player2;
    private Game? _game1;
    private Round? _round1;
    private Round? _round2;

    private DbContextOptions<DataContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [SetUp]
    public void Setup()
    {
        _options = CreateNewContextOptions();
        _player1 = new Player { Id = 1, ConnectionId = "123", Username = "Test Player" };
        _player2 = new Player { Id = 2, ConnectionId = "456", Username = "Test Player 2" };
        _game1 = new Game { Id = 1, Name = "Test Game" };
        _round1 = new Round { Id = 1, IsCurrent = true, Word = "Test Word", GameId = 1, DasherId = 1 };
        _round2 = new Round { Id = 2, IsCurrent = false, Word = "Test Word", GameId = 1, DasherId = 2 };
    }

    [Test]
    public async Task AddPlayer_ShouldAddPlayerToDatabaseAsync()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);

            // Act
            repository.AddPlayer(_player1!);
            await context.SaveChangesAsync();
        }

        // Assert
        using (var context = new DataContext(_options!))
        {
            var player = await context.Players.FindAsync(1);
            Assert.That(player, Is.Not.Null);
            Assert.That(player?.Id, Is.EqualTo(1));
            Assert.That(player?.ConnectionId, Is.EqualTo("123"));
            Assert.That(player?.Username, Is.EqualTo("Test Player"));
        }
    }

    [Test]
    public async Task GetPlayerAsync_ShouldReturnPlayerById()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayerAsync(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.ConnectionId, Is.EqualTo("123"));
            Assert.That(result?.Username, Is.EqualTo("Test Player"));
        }
    }

    [Test]
    public async Task GetPlayerByConnIdAsync_ShouldReturnPlayerByConnId()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayerByConnIdAsync("123");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.ConnectionId, Is.EqualTo("123"));
            Assert.That(result?.Username, Is.EqualTo("Test Player"));
        }
    }

    [Test]
    public async Task GetPlayerByUsernameAsync_ShouldReturnPlayerByUsername()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayerByUsernameAsync("Test Player");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.ConnectionId, Is.EqualTo("123"));
            Assert.That(result?.Username, Is.EqualTo("Test Player"));
        }
    }

    [Test]
    public async Task GivePoints_ShouldGivePlayersPoints()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            await repository.GivePointsAsync(1, 10);
            await context.SaveChangesAsync();
        }


        // Assert
        using (var context = new DataContext(_options!))
        {
            var result = await context.Players.FindAsync(1);
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.TotalPoints, Is.EqualTo(10));
        }
    }

    [Test]
    public async Task GetPlayerGameAsync_ShouldReturnPlayerGameByConnId()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            _game1!.Players.Add(_player1!);
            context.Games.Add(_game1);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayerGameAsync(_player1!.ConnectionId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Test Game"));
        }
    }

    [Test]
    public async Task GetPlayerGameAsync_ShouldReturnPlayerGame()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            _game1!.Players.Add(_player1!);
            context.Games.Add(_game1);
            context.Rounds.Add(_round1!);
            context.Rounds.Add(_round2!);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayerGameAsync(_player1!);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Id, Is.EqualTo(1));
            Assert.That(result?.Name, Is.EqualTo("Test Game"));
            Assert.That(result?.CurrentRound?.Id, Is.EqualTo(1));
            Assert.That(result?.CurrentRound?.Word, Is.EqualTo("Test Word"));
            Assert.That(result?.CurrentRound?.DasherId, Is.EqualTo(1));
        }
    }

    [Test]
    public async Task GetPlayersNotInGameAsync_ShouldReturnPlayersNotInGame()
    {
        // Arrange
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            context.Players.Add(_player1!);
            context.Players.Add(_player2!);
            _game1!.Players.Add(_player1!);
            context.Games.Add(_game1);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new DataContext(_options!))
        {
            var repository = new PlayerRepository(context);
            var result = await repository.GetPlayersNotInGameAsync();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result?.FirstOrDefault()?.Id, Is.EqualTo(2), "Player 2 should be returned");
            Assert.That(result?.FirstOrDefault()?.ConnectionId, Is.EqualTo("456"), "Player 2 should be returned");
            Assert.That(result?.FirstOrDefault()?.Username, Is.EqualTo("Test Player 2"), "Player 2 should be returned");
        }
    }
}
