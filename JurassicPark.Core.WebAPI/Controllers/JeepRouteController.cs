using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/jeep-routes")]
public class JeepRouteController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllRoutes(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var routes = await gameService.GetRoutes(game.GetValueOrThrow());
        List<JeepRouteDto> returns = new List<JeepRouteDto>();
        foreach (var route in routes)
        {
            returns.Add(await route.ToDtoAsync(gameService));
        }
        
        return Ok(returns);
    }
}