using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/animal-groups")]
public class AnimalGroupsController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAnimalGroups(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var groups = await gameService.GetGroups(game.GetValueOrThrow());
        List<AnimalGroupDto> returns = new List<AnimalGroupDto>();
        foreach (var group in groups)
        {
            returns.Add(await group.ToDtoAsync(gameService));
        }

        return Ok(returns);
    }
}