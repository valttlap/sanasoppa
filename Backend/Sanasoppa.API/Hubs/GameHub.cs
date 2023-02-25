using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Entities;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Hubs
{
    public class GameHub : Hub
    {
        private readonly IUnitOfWork _uow;

        public GameHub(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<int> CreateGame(string username)
        {
            username = username.Sanitize();
            // create new game and player
            var game = new Game
            {
                ConnectionId = await _uow.GameRepository.GetNextConnectionId()
            };
            
            var player = new Player
            {
                ConnectionId = Context.ConnectionId,
                Name = username,
                IsDasher = true
            };
            game.Players.Add(player);
            _uow.GameRepository.AddGame(game);

            if (await _uow.Complete())
            {
                // add player to group for this game
                await Groups.AddToGroupAsync(Context.ConnectionId, game.ConnectionId.ToString());

                // return game id to player
                return game.ConnectionId;
            } else
            {
                throw new Exception("An error occured while creating a game");
            }
        }

        public async Task<IEnumerable<string>> GetPlayers(string connectionId)
        {
            connectionId = connectionId.Sanitize();
            var game = await _uow.GameRepository.GetByConnectionIdAsync(int.Parse(connectionId));

            if (game == null)
            {
                throw new ArgumentNullException(nameof(connectionId), "Invalid game ID");
            }
            return (await _uow.GameRepository.GetPlayersAsync(game.Id)).Select(p => p?.Name);

        }

        public async Task JoinGame(string connectionId, string username)
        {
            connectionId = connectionId.Sanitize();
            username = username.Sanitize();
            try
            {
                var game = await _uow.GameRepository.GetByConnectionIdAsync(int.Parse(connectionId));

                if (game == null)
                {
                    throw new ArgumentNullException(nameof(connectionId), "Invalid game ID");
                }

                var player = new Player
                {
                    ConnectionId = Context.ConnectionId,
                    Name = username,
                    IsDasher = false
                };

                game.Players.Add(player);
                _uow.GameRepository.Update(game);

                if (await _uow.Complete())
                {
                    // add player to group for this game
                    await Groups.AddToGroupAsync(player.ConnectionId, game.ConnectionId.ToString());

                    // notify all players in the game that a new player has joined
                    var playerNames = (await _uow.GameRepository.GetPlayersAsync(game.Id)).Select(p => p?.Name);
                    await Clients.Group(game.ConnectionId.ToString()).SendAsync("PlayerJoined", playerNames);
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
            var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
            game.GameStarted = true;
            _uow.GameRepository.Update(game);

            if (await _uow.Complete())
            {
                var dasher = await _uow.GameRepository.GetDasherAsync(game);
                await Clients.GroupExcept(game.ConnectionId.ToString(), dasher.ConnectionId).SendAsync("GameStarted");
                await Clients.Client(dasher.ConnectionId).SendAsync("StartRound");
            }
        }

        public async Task StartRound(string word)
        {
            word = word.Sanitize();
            var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
            
            var dasher = await _uow.GameRepository.GetDasherAsync(game);
            if (dasher.Id != player.Id)
            {
                throw new ArgumentException("Player is not the dasher");
            }

            var newRound = new Round
            {
                Word = word,
            };

            game.CurrentRound = newRound;
            _uow.GameRepository.Update(game);

            if (await _uow.Complete())
            {
                await Clients.Group(game.ConnectionId.ToString()).SendAsync("RoundStarted", word);
            }
        }

        public async Task GiveExplanation(string explanation)
        {
            explanation = explanation.Sanitize();
            var (game, player) = await GetGameAndPlayer(Context.ConnectionId);
            var newExplanation = new Explanation
            {
                Text = explanation,
                IsRight = player.IsDasher,
                PlayerId = player.Id
            };

            _uow.RoundRepository.AddExplanation(game.CurrentRound!, newExplanation);

            if (await _uow.Complete())
            {
                await Clients.Group(game.ConnectionId.ToString()).SendAsync("ExplanationGiven", player);
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
