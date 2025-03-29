using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/jeeps")]
public class JeepController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllJeeps(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var jeeps = await gameService.GetJeeps(game.GetValueOrThrow());
        List<JeepDto> returns = new List<JeepDto>();
        foreach (var jeep in jeeps)
        {
            returns.Add(await jeep.ToDtoAsync(gameService));
        }
        
        return Ok(returns);
    }

    [HttpGet("{jeepId:long}")]
    public async Task<IActionResult> GetJeep(long gameId, long jeepId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var jeep = await gameService.GetJeepById(jeepId);
        if (jeep.IsError) return jeep.GetErrorOrThrow().ToHttpResult();
        
        return Ok(await jeep.GetValueOrThrow().ToDtoAsync(gameService));
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> PurchaseJeep(long gameId, [FromBody] decimal jeepPrice)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var jeep = await gameService.PurchaseJeep(game.GetValueOrThrow(), jeepPrice);
        if (jeep.IsError) return jeep.GetErrorOrThrow().ToHttpResult();
        
        return Ok(await jeep.GetValueOrThrow().ToDtoAsync(gameService));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateJeep(long gameId, long id, [FromBody] UpdateJeepRequest request)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();

        var jeep = await gameService.GetJeepById(id);
        if (jeep.IsError) return jeep.GetErrorOrThrow().ToHttpResult();

        JeepRoute? route = null;
        if (request.RouteId.HasValue)
        {
            var result = await gameService.GetRouteById(request.RouteId.Value);
            if (result.IsError) return result.GetErrorOrThrow().ToHttpResult();

            route = result.GetValueOrThrow();
        }

        var value = jeep.GetValueOrThrow();
        value.RouteProgression = request.RouteProgression;
        value.SeatedVisitors = request.SeatedVisitors;
        value.Route = route;
        value.RouteId = route?.Id;

        return (await gameService.UpdateJeep(value)).ToHttpResult();
    }
    
    [HttpDelete("{id:long}/sell")]
    public async Task<IActionResult> SellJeep(long gameId, long id, [FromBody] decimal refundPrice)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var jeep = await gameService.GetJeepById(id);
        if (jeep.IsError) return jeep.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.SellJeep(game.GetValueOrThrow(), 
            jeep.GetValueOrThrow(), refundPrice);
        return result.ToHttpResult();
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteJeep(long gameId, long id)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var jeep = await gameService.GetJeepById(id);
        if (jeep.IsError) return jeep.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.DeleteJeep(jeep.GetValueOrThrow());
        return result.ToHttpResult();
    }
}