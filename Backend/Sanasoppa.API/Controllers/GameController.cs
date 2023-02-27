using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Controllers
{
    [Authorize(Policy = "RequireMemberRole")]
    public class GameController : BaseApiController
    {
        private readonly IUnitOfWork _uow;

        public GameController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<GameDto>> GetGame(string name)
        {
            var game = await _uow.GameRepository.GetGameByNameAsync(name);
            if (game == null)
            {
                return NotFound();
            }
            return Ok(game);
        }

        [HttpGet("not-started")]
        public async Task<ActionResult<GameDto>> GetNotStartedGames()
        {
            var games = await _uow.GameRepository.GetNotStartedGamesAsync();

            return Ok(games);
        }

        [HttpGet("players/{name}")]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetGamePlayers(string name)
        {
            var games = await _uow.GameRepository.GetGamePlayersAsync(name);
            return Ok(games);
        }
    }
}
