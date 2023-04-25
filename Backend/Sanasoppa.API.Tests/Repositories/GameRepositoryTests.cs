using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Helpers;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Tests.Repositories;

[TestFixture]
public class GameRepositoryTests
{
    private DbContextOptions<DataContext>? _options;
    private DataContext? _context;
    private IGameRepository? _repository;
    private IMapper? _mapper;
    private Faker _faker = new Faker("fi");

    [SetUp]
    public void Setup()
    {
        _options = RepostitoryTestUtils.CreateNewContextOptions();
        _context = new DataContext(_options!);
        _mapper = ConfigureMapper();
        _repository = new GameRepository(_context, _mapper);

    }

    private IMapper ConfigureMapper()
    {
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfiles());
        });
        return mapperConfig.CreateMapper();
    }


    [TearDown]
    public async Task TearDownAsync()
    {
        foreach (var entity in _context!.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        await _context!.Database.EnsureDeletedAsync();

        _context!.Dispose();
    }

    [Test]
    public async Task GetGamesAsync_ShouldReturnAllGames()
    {
        // Arrange
        var game1 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var game2 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var game3 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };

        _context!.Games.AddRange(game1, game2, game3);
        await _context!.SaveChangesAsync();

        // Act
        IEnumerable<Game?> result;
        result = await _repository!.GetGamesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(3));
        Assert.That(result, Has.Some.Matches<Game?>(g => g?.Id == 1 && g.Name == game1.Name && g.GameState == GameState.NotStarted));
        Assert.That(result, Has.Some.Matches<Game?>(g => g?.Id == 2 && g.Name == game2.Name && g.GameState == GameState.NotStarted));
        Assert.That(result, Has.Some.Matches<Game?>(g => g?.Id == 3 && g.Name == game3.Name && g.GameState == GameState.NotStarted));
    }


    [Test]
    public async Task GetGameAsync_ShouldReturnGameById()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetGameAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
    }

    [Test]
    public async Task GetGameAsync_ShouldReturnGameByName()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetGameAsync(game.Name);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
    }

    [Test]
    public async Task GetGameWithPlayersAsync_ShouldReturnGameWithPlayersById()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        game.Players.Add(player);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetGameWithPlayersAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
        Assert.That(result.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints && p.GameId == result.Id));
    }

    [Test]
    public async Task GetGameWithPlayersAsync_ShouldReturnGameWithPlayerByName()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        game.Players.Add(player);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetGameWithPlayersAsync(game.Name);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
        Assert.That(result.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints && p.GameId == result.Id));
    }

    [Test]
    public async Task GetWholeGameAsync_ShouldReturnGameWithEverythingById()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        var round = new Round
        {
            IsCurrent = true,
            DasherId = 1,
            Word = _faker.Lorem.Word()
        };
        game.Players.Add(player);
        game.Rounds.Add(round);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetWholeGameAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
        Assert.That(result.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints));
        Assert.That(result.Rounds, Is.Not.Null);
        Assert.That(result.Rounds.Count(), Is.EqualTo(1));
        Assert.That(result.Rounds, Has.Some.Matches<Round>(r => r.Id == 1 && r.Word == round.Word && r.GameId == round.GameId && r.IsCurrent == round.IsCurrent));
    }

    [Test]
    public async Task GetWholeGameAsync_ShouldReturnGameWithEverythingByName()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        var round = new Round
        {
            IsCurrent = true,
            DasherId = 1,
            Word = _faker.Lorem.Word()
        };
        game.Players.Add(player);
        game.Rounds.Add(round);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        result = await _repository!.GetWholeGameAsync(game.Name);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
        Assert.That(result.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints));
        Assert.That(result.Rounds, Is.Not.Null);
        Assert.That(result.Rounds.Count(), Is.EqualTo(1));
        Assert.That(result.Rounds, Has.Some.Matches<Round>(r => r.Id == 1 && r.Word == round.Word && r.GameId == round.GameId && r.IsCurrent == round.IsCurrent));
    }

    [Test]
    public async Task GetNotStartedGamesAsync_ShouldReturnNotStartedGames()
    {
        // Arrange
        var game1 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var game2 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.GivingExplanations };
        var game3 = new Game { Name = _faker.Lorem.Word(), GameState = GameState.DasherValuingExplanations };

        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };

        game1.Players.Add(player);

        _context!.Games.AddRange(game1, game2, game3);
        await _context!.SaveChangesAsync();

        // Act
        IEnumerable<GameDto?> result;
        result = await _repository!.GetNotStartedGamesAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(1));
        Assert.That(result, Has.Some.Matches<GameDto>(g => g.Name == game1!.Name && g.Players == 1));
    }

    [Test]
    public async Task GetDasherAsync_ShouldReturnCurrentDasher()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        var player2 = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        var round = new Round
        {
            IsCurrent = true,
            DasherId = 1,
            Word = _faker.Lorem.Word()
        };
        game.Players.Add(player);
        game.Players.Add(player2);
        game.Rounds.Add(round);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Player? result;
        result = await _repository!.GetDasherAsync(game);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Username, Is.EqualTo(player.Username));
        Assert.That(result.ConnectionId, Is.EqualTo(player.ConnectionId));
    }


    [Test]
    public async Task GameExistsAsync_ShouldReturnTrueIfGameExists()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();
        // Act
        bool result;
        result = await _repository!.GameExistsAsync(1);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task GameExistsAsync_ShouldReturnFalseIfGameNotExists()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        bool result;
        result = await _repository!.GameExistsAsync(2);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AddGame_ShouldAddGameToDatabaseAsync()
    {
        // Arrange
        var game = new Game()
        {
            Name = _faker.Lorem.Word(),
            GameState = GameState.NotStarted
        };

        // Act
        _repository!.AddGame(game);
        await _context!.SaveChangesAsync();
        Game? result;
        result = await _context!.Games.FindAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));

    }

    [Test]
    public async Task AddPlayerToGameAsync_ShouldAddPlayerToGameWithPlayerAndGame()
    {
        // Arrange
        var game = new Game()
        {
            Name = _faker.Lorem.Word(),
            GameState = GameState.NotStarted
        };
        var player = new Player()
        {
            Username = "Test",
            ConnectionId = "Test",
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        _repository!.AddPlayerToGame(game, player);
        await _context!.SaveChangesAsync();
        Game? result;
        result = await _context!.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == 1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints));
    }

    [Test]
    public async Task HasGameEnded_ShouldReturnTrueForEndedGame()
    {
        // Arrange
        var game = new Game()
        {
            Name = _faker.Lorem.Word(),
            GameState = GameState.NotStarted
        };
        var player = new Player()
        {
            Username = "Test",
            ConnectionId = "Test",
            IsHost = true,
            IsOnline = true,
            TotalPoints = 21
        };
        game.Players.Add(player);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        bool result;
        result = _repository!.HasGameEnded(game);

        // Assert
        Assert.That(result, Is.True);
    }

    [Test]
    public async Task HasGameEnded_ShouldReturnFalseForNotEndedGame()
    {
        // Arrange
        var game = new Game()
        {
            Name = _faker.Lorem.Word(),
            GameState = GameState.NotStarted
        };
        var player = new Player()
        {
            Username = "Test",
            ConnectionId = "Test",
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        game.Players.Add(player);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        bool result;
        result = _repository!.HasGameEnded(game);

        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task AddPlayerToGameAsync_ShouldAddExistingPlayerToGame()
    {
        // Arrange
        var game = new Game()
        {
            Name = _faker.Lorem.Word(),
            GameState = GameState.NotStarted
        };
        var player = new Player()
        {
            Username = "Test",
            ConnectionId = "Test",
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        _context!.Games.Add(game);
        _context!.Players.Add(player);
        await _context!.SaveChangesAsync();

        // Act
        await _repository!.AddPlayerToGameAsync(game, player.Username);
        await _context!.SaveChangesAsync();
        Game? result;
        result = await _context!.Games.Include(g => g.Players).FirstOrDefaultAsync(g => g.Id == 1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Players, Is.Not.Null);
        Assert.That(result.Players.Count(), Is.EqualTo(1));
        Assert.That(result.Players, Has.Some.Matches<Player>(p => p.Id == 1 && p.Username == player.Username && p.ConnectionId == player.ConnectionId && p.IsHost == player.IsHost && p.IsOnline == player.IsOnline && p.TotalPoints == player.TotalPoints));
    }

    [Test]
    public async Task RemovePlayerFromGame_ShouldRemovePlayerFromGame()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        var player = new Player
        {
            Username = _faker.Internet.UserName(),
            ConnectionId = _faker.Random.Uuid().ToString(),
            IsHost = true,
            IsOnline = true,
            TotalPoints = 0
        };
        game.Players.Add(player);
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        _repository!.RemovePlayerFromGame(game, player.Username);
        await _context!.SaveChangesAsync();
        result = await _context.Games.FindAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.NotStarted));
        Assert.That(result.Players.Count(), Is.EqualTo(0));
    }

    [Test]
    public async Task StartGame_ShouldStartGame()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        _repository!.StartGame(game);
        await _context!.SaveChangesAsync();
        result = await _context.Games.FindAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo(game.Name));
        Assert.That(result.GameState, Is.EqualTo(GameState.WaitingDasher));
    }

    [Test]
    public async Task RemoveGame_ShouldRemoveGame()
    {
        // Arrange
        var game = new Game { Name = _faker.Lorem.Word(), GameState = GameState.NotStarted };
        _context!.Games.Add(game);
        await _context!.SaveChangesAsync();

        // Act
        Game? result;
        _repository!.RemoveGame(game);
        await _context!.SaveChangesAsync();
        result = await _context.Games.FindAsync(1);

        // Assert
        Assert.That(result, Is.Null);
    }





}