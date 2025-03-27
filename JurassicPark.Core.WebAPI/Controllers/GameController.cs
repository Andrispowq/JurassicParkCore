using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games")]
public class GameController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetSavedGamesAsync()
    {
        var games = await gameService.GetSavedGames();
        return Ok(games.Select(g => new SavedGameDto(g)));
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetSavedGameAsync(long id)
    {
        var game = await gameService.GetSavedGame(id);
        return game.ToOkResult(g => new SavedGameDto(g));
    }

    [HttpPost]
    public async Task<IActionResult> CreateGameAsync([FromBody] SavedGameCreateDto request)
    {
        var game = await gameService.CreateNewGame(request.Name,
            request.Difficulty, request.MapWidth, request.MapHeight);
        return game.ToOkResult(g => new SavedGameDto(g));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateSavedGameAsync(long id, [FromBody] SavedGameUpdateRequest request)
    {
        var game = await gameService.GetSavedGame(id);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var save = game.GetValueOrThrow();
        save.Difficulty = request.Difficulty;
        save.TimeOfDay = request.TimeOfDay;
        save.DaysPassed = request.DaysPassed;
        save.VisitorSatisfaction = request.VisitorSatisfaction;
        save.HoursSinceGoalMet = request.HoursSinceGoalMet;
        save.GameSpeed = request.GameSpeed;
        save.GameState = request.GameState;

        var result = await gameService.UpdateGame(save);
        return result.ToHttpResult();
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> CreateGameAsync(long id)
    {
        var game = await gameService.GetSavedGame(id);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.DeleteGame(game.GetValueOrThrow());
        return result.ToHttpResult();
    }
}