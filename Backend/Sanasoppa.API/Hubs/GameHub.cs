using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Hubs
{
    [Authorize(Policy = "RequireMemberRole")]
    public class GameHub : Hub
    {
        private readonly IUnitOfWork _uow;

        public GameHub(IUnitOfWork uow)
        {
            _uow = uow;
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
            }
            else if (player.ConnectionId != Context.ConnectionId)
            {
                player.ConnectionId = Context.ConnectionId;
                _uow.PlayerRepository.Update(player);
            }

            player.IsOnline = true;
            _uow.PlayerRepository.Update(player);

            var httpContext = Context.GetHttpContext();
            var gameName = httpContext?.Request.Query["game"].ToString();

            if (string.IsNullOrWhiteSpace(gameName))
            {
                throw new HubException("Name of the game was not given");
            }

            gameName = gameName.Sanitize();
            
            await _uow.GameRepository.AddPlayerToGameAsync(gameName, player);
            await Groups.AddToGroupAsync(player.ConnectionId, gameName!);

            if (_uow.HasChanges()) await _uow.Complete();

            var players = await _uow.GameRepository.GetPlayerDtosAsync(gameName);
            await Clients.Group(gameName).SendAsync("PlayerJoined", players);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var player = await _uow.PlayerRepository.GetPlayerByConnIdAsync(Context.ConnectionId);
            if (player == null) return;
            player.IsOnline = false;
            if (player.GameId == null)
            {
                await _uow.Complete();
                return;
            }
            var game = await _uow.GameRepository.GetWholeGame((int)player.GameId);
            if (game == null) return;
            if (game.Players.Where(p => p != player).Any(p => p.IsOnline)) return;
            _uow.GameRepository.RemoveGameAsync(game);
        }


        public async Task<IEnumerable<PlayerDto?>> GetPlayers(string gameName)
        {
            var game = await _uow.GameRepository.GetGameAsync(gameName.Sanitize());

            if (game == null)
            {
                throw new ArgumentException("Invalid game name", nameof(gameName));
            }
            return await _uow.GameRepository.GetPlayerDtosAsync(game.Id);

        }


        /// <summary>
        /// It starts a game.
        /// </summary>
        /// <param name="gameName">The name of the game you want to start.</param>
        public async Task StartGame(string gameName)
        {
            await _uow.GameRepository.StartGameAsync(gameName.Sanitize());
            

            if (await _uow.Complete())
            {
                var dasher = await _uow.GameRepository.GetDasherAsync(gameName);
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

            if (!player.IsDasher ?? false)
            {
                throw new ArgumentException("Only dasher can start the round");
            }

            var newRound = new Round
            {
                Game = game,
                GameId = game.Id,
                Word = word,
            };
            _uow.RoundRepository.AddRound(newRound);

            game.CurrentRound = newRound;
            game.GameState = 3;
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
                IsRight = player.IsDasher ?? false,
                Round = game.CurrentRound!,
                RoundId = game.CurrentRound!.Id,
                Player = player,
                PlayerId = player.Id
            };
            _uow.RoundRepository.AddExplanation(game.CurrentRound!, newExplanation);

            if (await _uow.Complete())
            {
                await Clients.Group(game.Name).SendAsync("ExplanationGiven", player);
            }
        }

        public async Task EndRound()
        {
            var (game, _) = await GetGameAndPlayer(Context.ConnectionId);
            game.CurrentRound = null;
            game.GameState = 2;
            _uow.GameRepository.Update(game);
            var currentDasher = _uow.GameRepository.GetDasherAsync(game);
            var nextDasher = ChangeDasher(game, currentDasher!);
            if (await _uow.Complete())
            {
                var waitDasherTask = Clients.GroupExcept(game.Name, nextDasher.ConnectionId).SendAsync("WaitDasher", nextDasher.Username);
                var startRoundTask = Clients.Client(nextDasher.ConnectionId).SendAsync("StartRound");
                Task.WaitAll(waitDasherTask, startRoundTask);
            }
        }

        private Player ChangeDasher(Game game, Player previousDasher)
        {
            var players = game.Players.OrderBy(p => p.Id).ToList();
            var index = players.FindIndex(p => p == previousDasher);
            var nextDasher = players.Skip(index + 1)
                .FirstOrDefault(p => !p.IsDasher ?? true) ?? players.FirstOrDefault(p => !p.IsDasher ?? true);
            nextDasher!.IsDasher = true;
            _uow.PlayerRepository.Update(nextDasher);
            previousDasher.IsDasher = false;
            _uow.PlayerRepository.Update(previousDasher);
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

            var game = await _uow.GameRepository.GetWholeGame((int)player.GameId);
            
            if (game == null)
            {
                throw new InvalidOperationException("Game not found for connection ID: " + connectionId);
            }

            return new Tuple<Game, Player>(game, player);
        }

    }
}
