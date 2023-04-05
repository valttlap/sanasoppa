using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Hubs;

[Authorize(Policy = "RequireMemberRole")]
public class GameHub : Hub
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GameHub(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        var username = Context.User!.GetUsername()!;
        var player = await _uow.PlayerRepository.GetPlayerByUsernameAsync(username);

        if (player == null)
        {
            player = new Player
            {
                ConnectionId = Context.ConnectionId,
                Username = username,
            };
            _uow.PlayerRepository.AddPlayer(player);
            await _uow.Complete();
        }
        else if (player.ConnectionId != Context.ConnectionId)
        {
            player.ConnectionId = Context.ConnectionId;
            _uow.PlayerRepository.Update(player);
        }

        player.IsOnline = true;
        _uow.PlayerRepository.Update(player);

        var gameName = Context
            .GetHttpContext()?
            .Request
            .Query["game"]
            .ToString()?
            .Sanitize();

        if (string.IsNullOrWhiteSpace(gameName))
        {
            throw new HubException("Name of the game was not given");
        }

        var game = await _uow.GameRepository.GetGameWithPlayersAsync(gameName);
        if (game == null)
        {
            throw new HubException("Game doesn't exist");
        }

        _uow.GameRepository.AddPlayerToGame(game, player);
        await Groups.AddToGroupAsync(player.ConnectionId, gameName!);

        if (_uow.HasChanges()) await _uow.Complete();

        var players = _mapper.Map<ICollection<PlayerDto>>(game);
        await Clients.Group(gameName).SendAsync("PlayerJoined", players);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var player = await _uow.PlayerRepository.GetPlayerByConnIdAsync(Context.ConnectionId);

        if (player == null)
        {
            return;
        }

        player.IsOnline = false;
        _uow.PlayerRepository.Update(player);

        if (player.GameId == null)
        {
            await _uow.Complete();
            return;
        }

        var game = await _uow.GameRepository.GetGameWithPlayersAsync(player.GameId.Value);

        if (game == null)
        {
            return;
        }

        var otherOnlinePlayers = game.Players.Where(p => p != player && p.IsOnline).ToList();

        if (otherOnlinePlayers.Any())
        {
            if (game.HostId == player.Id)
            {
                game.HostId = otherOnlinePlayers.First().Id;
                _uow.GameRepository.Update(game);
            }
        }
        else
        {
            _uow.GameRepository.RemoveGame(game);
        }

        if (_uow.HasChanges())
        {
            await _uow.Complete();
        }
    }



    public async Task<IEnumerable<PlayerDto?>> GetPlayers(string gameName)
    {
        var game = await _uow.GameRepository.GetGameWithPlayersAsync(gameName.Sanitize());

        if (game == null)
        {
            throw new ArgumentException("Invalid game name", nameof(gameName));
        }
        return _mapper.Map<ICollection<PlayerDto>>(game);

    }


    /// <summary>
    /// It starts a game.
    /// </summary>
    /// <param name="gameName">The name of the game you want to start.</param>
    public async Task StartGame(string gameName)
    {
        var game = await _uow.GameRepository.GetGameAsync(gameName.Sanitize());
        if (game == null)
        {
            throw new ArgumentException("Invalid game name", nameof(gameName));
        }
        _uow.GameRepository.StartGame(game);


        if (await _uow.Complete())
        {
            var dasher = await _uow.PlayerRepository.GetPlayerAsync(game.HostId);
            var gameStartedTask = Clients.GroupExcept(gameName, dasher!.ConnectionId).SendAsync("WaitDasher", dasher.Username);
            var startRoundTask = Clients.Client(dasher.ConnectionId).SendAsync("StartRound");
            Task.WaitAll(gameStartedTask, startRoundTask);
        }
    }

    /// <summary>
    /// It starts a new round of the game.
    /// </summary>
    /// <param name="word">The word that the players will be explaining.</param>
    public async Task StartRound(string word)
    {
        word = word.Sanitize();

        var (game, player) = await GetGameAndPlayer(Context.ConnectionId);

        if (game.CurrentRoundId == null)
        {
            if (game.HostId != player.Id)
            {
                throw new ArgumentException("Only host can start the first round");
            }

            game.CurrentRound = new Round
            {
                Game = game,
                GameId = game.Id,
                Word = word,
                Dasher = player,
                DasherId = player.Id
            };
        }
        else
        {
            if (game.CurrentRound!.DasherId != player.Id)
            {
                throw new ArgumentException("Only dasher can start the round");
            }

            var players = game.Players.OrderBy(p => p.Id).ToList();
            var dasherIndex = players.FindIndex(p => p.Id == game.CurrentRound.DasherId);
            var nextDasherIndex = (dasherIndex + 1) % players.Count;
            var nextDasher = players[nextDasherIndex];

            game.CurrentRound = new Round
            {
                Game = game,
                GameId = game.Id,
                Word = word,
                Dasher = nextDasher,
                DasherId = nextDasher.Id
            };
        }

        game.GameState = GameState.GivingExplanations;
        _uow.GameRepository.Update(game);

        if (await _uow.Complete())
        {
            await Clients.Group(game.Name).SendAsync("RoundStarted", word);
        }
    }


    public async Task GiveExplanation(string explanation)
    {
        explanation = explanation.Sanitize();
        var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
        var newExplanation = new Explanation
        {
            Text = explanation,
            IsRight = game.CurrentRound!.DasherId == player.Id,
            Round = game.CurrentRound!,
            RoundId = game.CurrentRound!.Id,
            Player = player,
            PlayerId = player.Id
        };
        _uow.RoundRepository.AddExplanation(game.CurrentRound!, newExplanation);

        if (await _uow.Complete())
        {
            await Clients.Group(game.Name).SendAsync("ExplanationGiven", player);
            if (game.CurrentRound!.Explanations.Count == game.Players.Count)
            {
                var dasher = await _uow.GameRepository.GetDasher(game);
                var round = await _uow.RoundRepository.GetRoundWithExplanationsAsync(game.CurrentRound.Id);
                var playersTask = Clients.GroupExcept(game.Name, dasher!.ConnectionId).SendAsync("AllExplanationsGiven");
                var dasherTask = Clients.Client(dasher!.ConnectionId).SendAsync("HandleExplanations", _mapper.Map<ICollection<ExplanationDto>>(round!.Explanations));
                game.GameState = GameState.DasherValuingExplanations;
                _uow.GameRepository.Update(game);
                var saveTask = _uow.Complete();
                Task.WaitAll(playersTask, dasherTask, saveTask);
            }
        }
    }

    public async Task ValuateExplanations(ICollection<int> RightPlayerIds, bool hasDuplicates)
    {
        if (RightPlayerIds.Count > 1) hasDuplicates = true;
        var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
        if (game.CurrentRound!.DasherId != player.Id)
        {
            throw new ArgumentException("Only dasher can valuate explanations");
        }
        foreach (var playerId in RightPlayerIds)
        {
            await _uow.PlayerRepository.GivePointsAsync(playerId, 2);
            await _uow.ExplanationRepository.RemoveExplanationAsync(playerId, game.CurrentRound.Id);
        }

        if (hasDuplicates)
        {
            await EndRound();
            return;
        }
        game.GameState = GameState.VotingExplanations;
        _uow.GameRepository.Update(game);

        var round = await _uow.RoundRepository.GetRoundWithExplanationsAsync(game.CurrentRound!.Id);

        var dasherTask = Clients.Client(player.ConnectionId).SendAsync("WaitResults");
        var playersTask = Clients.GroupExcept(game.Name, player.ConnectionId).SendAsync("VoteExplanations", _mapper.Map<ICollection<VoteExplanationDto>>(round!.Explanations));
        var saveTask = _uow.Complete();
        Task.WaitAll(dasherTask, playersTask, saveTask);
    }

    public async Task CastVote(int explanationId)
    {
        var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
        var explanation = await _uow.ExplanationRepository.GetExplanationAsync(explanationId);
        if (explanation == null)
        {
            throw new ArgumentException("Invalid explanation id", nameof(explanationId));
        }
        var vote = new Vote
        {
            ExplanationId = explanation.Id,
            PlayerId = player.Id,
            RoundId = game.CurrentRound!.Id
        };
        _uow.VoteRepository.AddVote(vote);
        if (await _uow.Complete())
        {
            await Clients.Group(game.Name).SendAsync("VoteCast", player.Username);
            if (game.CurrentRound!.Votes.Count == game.CurrentRound!.Explanations.Count - 1)
            {
                await EndRound();
            }
        }
    }


    public async Task EndRound()
    {
        var (game, _) = await GetGameAndPlayer(Context.ConnectionId);
        await CalculatePoints(game.CurrentRound!);
        game.CurrentRound = null;
        game.GameState = GameState.WaitingDasher;
        _uow.GameRepository.Update(game);
        var nextDasher = GetNextDasher(game);
        if (await _uow.Complete())
        {
            var waitDasherTask = Clients.GroupExcept(game.Name, nextDasher.ConnectionId).SendAsync("WaitDasher", nextDasher.Username);
            var startRoundTask = Clients.Client(nextDasher.ConnectionId).SendAsync("StartRound");
            Task.WaitAll(waitDasherTask, startRoundTask);
        }
    }

    private async Task CalculatePoints(Round round)
    {
        var explanations = await _uow.ExplanationRepository.GetRoundExplanationsWithVotesAsync(round.Id);
        var rightExplanation = explanations.FirstOrDefault(e => e.IsRight);
        var rightVotesCount = rightExplanation!.Votes.Count;
        if (rightVotesCount == 0)
        {
            await _uow.PlayerRepository.GivePointsAsync(round.DasherId, 2);
            await _uow.Complete();
            return;
        }

        foreach (var vote in rightExplanation.Votes)
        {
            await _uow.PlayerRepository.GivePointsAsync(vote.PlayerId, 1);
        }
        foreach (var explanation in explanations.Where(e => !e.IsRight && e.Votes.Count > 0))
        {
            foreach (var vote in explanation.Votes)
            {
                await _uow.PlayerRepository.GivePointsAsync(explanation.PlayerId, 1);
            }
        }
        await _uow.Complete();
    }

    private static Player GetNextDasher(Game game)
    {
        Player nextDasher;
        if (game.CurrentRound == null)
        {
            nextDasher = game.Host;
        }
        else
        {
            var players = game.Players.OrderBy(p => p.Id).ToList();
            var dasherIndex = players.FindIndex(p => p.Id == game.CurrentRound.DasherId);
            var nextDasherIndex = (dasherIndex + 1) % players.Count;
            nextDasher = players[nextDasherIndex];
        }
        return nextDasher;
    }



    private async Task<Tuple<Game, Player>> GetGameAndPlayer(string connectionId)
    {
        var player = await _uow.PlayerRepository.GetPlayerByConnIdAsync(connectionId);
        if (player == null)
        {
            throw new InvalidOperationException("Player not found for connection ID: " + connectionId);
        }
        if (player.GameId == null)
        {
            throw new InvalidOperationException("Player is not in any game");
        }

        var game = await _uow.GameRepository.GetWholeGameAsync(player.GameId.Value);

        if (game == null)
        {
            throw new InvalidOperationException("Game not found for connection ID: " + connectionId);
        }

        return new Tuple<Game, Player>(game, player);
    }

}
