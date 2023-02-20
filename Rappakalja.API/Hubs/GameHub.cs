using Microsoft.AspNetCore.SignalR;
using Rappakalja.API.Data.Repositories;
using Rappakalja.API.Entities;
using Rappakalja.API.Interfaces;

namespace Rappakalja.API.Hubs
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


    }
}
