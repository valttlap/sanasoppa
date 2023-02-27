using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
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

            if (_uow.HasChanges()) await _uow.Complete();

            await Clients.Caller.SendAsync("Joined to gamehub");
        }

        private async Task GameListChanged()
        {
            var gamesTask = _uow.GameRepository.GetNotStartedGamesAsync();
            var playersTask = _uow.PlayerRepository.GetPlayersNotInGameAsync();
            await Task.WhenAll(playersTask, gamesTask);
            var players = playersTask.Result;
            var games = gamesTask.Result;
            var playerIds = players.Select(p => p.ConnectionId).ToList();
            await Clients.Clients(playerIds).SendAsync("GameListUpdated", games);
        }

        public async Task<string> CreateGame(string gameName)
        {
            var username = Context.User!.GetUsername();
            // create new game and player
            var game = new Game
            {
                Name = gameName.Sanitize(),
            };
            
            var player = await _uow.PlayerRepository.GetPlayerByUsernameAsync(username);
            if (player == null)
            {
                throw new InvalidOperationException("Player not found");
            }
            player.IsDasher = true;
            _uow.PlayerRepository.Update(player);
            game.Players.Add(player);
            _uow.GameRepository.AddGame(game);

            if (await _uow.Complete())
            {
                // add player to group for this game
                var addToGroupTask = Groups.AddToGroupAsync(player.ConnectionId, game.Name);
                var gameListChangedTask = GameListChanged();
                Task.WaitAll(addToGroupTask, gameListChangedTask);

                // return game id to player
                return game.Name;
            } else
            {
                throw new Exception("An error occured while creating a game");
            }
        }

        public async Task<IEnumerable<string?>> GetPlayers(string gameName)
        {
            var game = await _uow.GameRepository.GetGameByNameAsync(gameName.Sanitize());

            if (game == null)
            {
                throw new ArgumentNullException(nameof(gameName), "Invalid game name");
            }
            return (await _uow.GameRepository.GetPlayersAsync(game.Id)).Select(p => p?.Username);

        }

        public async Task JoinGame(string gameName)
        {
            try
            {
                var game = await _uow.GameRepository.GetGameByNameAsync(gameName.Sanitize());

                if (game == null)
                {
                    throw new ArgumentNullException(nameof(gameName), "Invalid game ID");
                }

                if (game.HasStarted)
                {
                    throw new InvalidOperationException("Can't join to game because it has already started");
                }

                Console.WriteLine(Context.ConnectionId);

                var player = await _uow.PlayerRepository.GetPlayerByConnIdAsync(Context.ConnectionId);
                if (player == null)
                {
                    throw new InvalidOperationException("Player not found");
                }

                game.Players.Add(player);
                _uow.GameRepository.Update(game);

                if (await _uow.Complete())
                {
                    // add player to group for this game
                    await Groups.AddToGroupAsync(player.ConnectionId, game.Name);

                    // notify all players in the game that a new player has joined
                    var players = (await _uow.GameRepository.GetGamePlayersAsync(game.Id));
                    await Clients.Group(game.Name).SendAsync("PlayerJoined", players);
                }
                else
                {
                    throw new Exception("An error occured while joining to game");
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public async Task StartGame()
        {
            var (game, _) = await GetGameAndPlayer(Context.ConnectionId);
            game.HasStarted = true;
            _uow.GameRepository.Update(game);

            if (await _uow.Complete())
            {
                var dasher = _uow.GameRepository.GetDasherAsync(game);
                var gameListChangedTask = GameListChanged();
                var gameStartedTask = Clients.GroupExcept(game.Name, dasher.ConnectionId).SendAsync("GameStarted");
                var startRoundTask = Clients.Client(dasher.ConnectionId).SendAsync("StartRound");
                Task.WaitAll(gameListChangedTask, gameStartedTask, startRoundTask);
            }
        }

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



        private async Task<Tuple<Game, Player>> GetGameAndPlayer(string connectionId)
        {
            var playerTask = _uow.PlayerRepository.GetPlayerByConnIdAsync(connectionId);
            var gameTask = _uow.PlayerRepository.GetPlayerGameAsync(connectionId);
            await Task.WhenAll(playerTask, gameTask);
            var player = playerTask.Result;
            var game = gameTask.Result;

            if (player == null)
            {
                throw new InvalidOperationException("Player not found for connection ID: " + connectionId);
            }

            if (game == null)
            {
                throw new InvalidOperationException("Game not found for connection ID: " + connectionId);
            }

            return new Tuple<Game, Player>(game, player);
        }

    }
}
