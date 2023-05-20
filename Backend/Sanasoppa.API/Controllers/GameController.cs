// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sanasoppa.API.DTOs;
using Sanasoppa.API.Interfaces;

namespace Sanasoppa.API.Controllers;
[Authorize]
public class GameController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public GameController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves the game with the specified name.
    /// </summary>
    /// <param name="gameName">The name of the game to retrieve.</param>
    /// <returns>An ActionResult containing a GameDto object if the game exists, or NotFound() if it does not.</returns>
    [HttpGet("{gameName}")]
    public async Task<ActionResult<GameDto>> GetGame(string gameName)
    {
        var game = await _uow.GameRepository.GetGameAsync(gameName).ConfigureAwait(false);
        if (game == null)
        {
            return NotFound();
        }
        var gameDto = _mapper.Map<GameDto>(game);
        return Ok(gameDto);
    }

    /// <summary>
    /// Retrieves a list of games that have not yet started.
    /// </summary>
    /// <returns>An ActionResult containing a list of GameDto objects representing the games that have not started.</returns>
    [HttpGet("not-started")]
    public async Task<ActionResult<GameDto>> GetNotStartedGames()
    {
        var games = await _uow.GameRepository.GetNotStartedGamesAsync().ConfigureAwait(false);

        return Ok(games);
    }

    /// <summary>
    /// Retrieves a list of players in the specified game.
    /// </summary>
    /// <param name="gameName">The name of the game to retrieve the players for.</param>
    /// <returns>An ActionResult containing a list of PlayerDto objects representing the players in the game.</returns>
    [HttpGet("players/{gameName}")]
    public async Task<ActionResult<ICollection<PlayerDto>>> GetGamePlayers(string gameName)
    {
        var game = await _uow.GameRepository.GetGameWithPlayersAsync(gameName).ConfigureAwait(false);
        if (game == null)
        {
            return NotFound("Game not found");
        }
        return Ok(
            _mapper.Map<ICollection<PlayerDto>>(game));
    }
}
