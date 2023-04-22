using Microsoft.EntityFrameworkCore;
using Sanasoppa.API.Data;
using Sanasoppa.API.Entities;

namespace Sanasoppa.API.Tests.Repositories;

[TestFixture]
public class GameRepositoryTests
{
    private DbContextOptions<DataContext>? _options;

    private Player? _player1;
    private Player? _player2;
    private Player? _player3;
    private Player? _player4;
    private Game? _game1;
    private Game? _game2;
    private Game? _game3;
    private Round? _game1Round1;
    private Round? _game1Round2;
    private Round? _game2Round1;
    private Round? _game2Round2;

    [SetUp]
    public void Setup()
    {
        _options = RepostitoryTestUtils.CreateNewContextOptions();
        _player1 = new Player { Id = 1, ConnectionId = "123", Username = "Test Player" };
        _player2 = new Player { Id = 2, ConnectionId = "456", Username = "Test Player 2" };
        _player3 = new Player { Id = 3, ConnectionId = "456", Username = "Test Player 2" };
        _player4 = new Player { Id = 4, ConnectionId = "456", Username = "Test Player 2" };
        _game1 = new Game { Id = 1, Name = "Test Game 1" };
        _game2 = new Game { Id = 2, Name = "Test Game 2" };
        _game3 = new Game { Id = 3, Name = "Test Game 3" };
        _game1Round1 = new Round { Id = 1, IsCurrent = false, Word = "Test Word", GameId = 1, DasherId = 1 };
        _game1Round2 = new Round { Id = 2, IsCurrent = true, Word = "Test Word", GameId = 1, DasherId = 2 };
        _game2Round1 = new Round { Id = 3, IsCurrent = false, Word = "Test Word", GameId = 1, DasherId = 1 };
        _game2Round2 = new Round { Id = 4, IsCurrent = true, Word = "Test Word", GameId = 1, DasherId = 2 };
    }

    [Test]
    public async Task GetGamesAsync_ShouldReturnAllGames()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetGameAsync_ShouldReturnGameById()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetGameAsync_ShouldReturnGameByName()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetGameWithPlayersAsync_ShouldReturnGameWithPlayerById()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetGameWithPlayersAsync_ShouldReturnGameWithPlayerByName()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetWholeGameAsync_ShouldReturnGameWithEverythingById()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetWholeGameAsync_ShouldReturnGameWithEverythingByName()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetNotStartedGamesAsync_ShouldReturnNotStartedGames()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task GetDasherAsync_ShouldReturnGamesDasher()
    {
        Assert.Ignore();
    }


    [Test]
    public async Task GameExistsAsync_ShouldCheckIfGameExistsById()
    {
        Assert.Ignore();
    }

    [Test]
    public void AddGame_ShouldAddGameToDatabase()
    {
        Assert.Ignore();
    }

    [Test]
    public async Task AddPlayerToGameAsync_ShouldAddPlayerToGame()
    {
        Assert.Ignore();
    }

    [Test]
    public void HasGameEndedAsync_ShouldCheckIfGameHasEnded()
    {
        Assert.Ignore();
    }

    [Test]
    public void AddPlayerToGame_ShouldAddPlayerToGame()
    {
        Assert.Ignore();
    }

    [Test]
    public void RemovePlayerFromGame_ShouldRemovePlayerFromGame()
    {
        Assert.Ignore();
    }

    [Test]
    public void StartGame_ShouldStartGame()
    {
        Assert.Ignore();
    }

    [Test]
    public void RemoveGame_ShouldRemoveGame()
    {
        Assert.Ignore();
    }





}