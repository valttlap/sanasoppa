using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.Data.Repositories;
using Sanasoppa.API.Entities;
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

        public async Task<IEnumerable<String?>> GetPlayers(string connectionId)
        {
            var game = await _uow.GameRepository.GetByConnectionIdAsync(int.Parse(connectionId));

            if (game == null)
            {
                throw new ArgumentException("Invalid game ID");
            }
            return (await _uow.GameRepository.GetPlayersAsync(game.Id)).Select(p => p?.Name);

        }

        public async Task JoinGame(string connectionId, string username)
        {
            try
            {
                var game = await _uow.GameRepository.GetByConnectionIdAsync(int.Parse(connectionId));

                if (game == null)
                {
                    throw new ArgumentException("Invalid game ID");
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
            var player = await _uow.PlayerRepository.GetPlayerByConnIdAsync(Context.ConnectionId);
            if (player == null)
            {
                throw new Exception("Player not found");
            }
            var game = await _uow.PlayerRepository.GetPlayerGameAsync(player);
            if (game == null)
            {
                throw new Exception("Game not found");
            }
            game.GameStarted = true;
            _uow.GameRepository.Update(game);

            if (await _uow.Complete())
            {
                await Clients.Group(game.ConnectionId.ToString()).SendAsync("GameStarted");
            }
        }

        public async Task StartRound(string word)
        {
            var playerTask = _uow.PlayerRepository.GetPlayerByConnIdAsync(Context.ConnectionId);
            var gameTask = _uow.PlayerRepository.GetPlayerGameAsync(Context.ConnectionId);
            await Task.WhenAll(playerTask, gameTask);
            var player = playerTask.Result;
            var game = gameTask.Result;
            if (player == null)
            {
                throw new Exception("Player not found");
            }
            
            if (game == null)
            {
                throw new Exception("Game not found");
            }
            
            var dasher = await _uow.GameRepository.GetDasherAsync(game);
            if (dasher.Id != player.Id)
            {
                throw new Exception("Player is not the dasher");
            }

            var newRound = new Round
            {
                Word = word,
            };

            game.CurrentRound = newRound;
            _uow.GameRepository.Update(game);

            if (await _uow.Complete())
            {
                await Clients.Group(game.ConnectionId.ToString()).SendAsync("RoundStarted", $"<b>{word}</b>");
            }

        }


    }
}
