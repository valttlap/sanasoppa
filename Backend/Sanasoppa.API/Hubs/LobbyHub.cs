using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Sanasoppa.API.Entities;
using Sanasoppa.API.EventArgs;
using Sanasoppa.API.Extensions;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Hubs
{
    [Authorize(Policy = "RequireMemberRole")]
    public class LobbyHub : Hub
    {
        private readonly IUnitOfWork _uow;

        public LobbyHub(IUnitOfWork uow)
        {
            _uow = uow;
            _uow.SubscribeToGameListChangedEvent(OnGameListChanged);
        }

        public async Task<string> CreateGame(string gameName)
        {
            gameName = gameName.Sanitize();
            if (await _uow.GameRepository.GameExistsAsync(gameName))
            {
                throw new ArgumentException($"The game with the name {{gameName}} already exists");
            }
            
            var game = new Game
            {
                Name = gameName,
                HasStarted = false,
                GameState = 0,
            };

            var player = await _uow.PlayerRepository.GetPlayerByUsernameAsync(Context.User!.GetUsername()!);
            if (player == null)
            {
                player = new Player
                {
                    ConnectionId = Context.ConnectionId,
                    IsDasher = true,
                    Username = Context.User!.GetUsername()!
                };
            }
            game.Players.Add(player);
            _uow.GameRepository.AddGame(game);
            if (await _uow.Complete())
            {
                return gameName;
            }
            else
            {
                throw new ApplicationException("Something went wrong while creating a game");
            }
        }

        private async void OnGameListChanged(object? sender, GameListChangedEventArgs e)
        {
            var games = await _uow.GameRepository.GetNotStartedGamesAsync();
            await Clients.All.SendAsync("GameListUpdated", games);
        }
    }
}
