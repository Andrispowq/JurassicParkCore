using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/map-objects")]
public class MapObjectController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMapObjects(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var mapObjects = await gameService.GetMapObjects(game.GetValueOrThrow());
        List<MapObjectDto> returns = new List<MapObjectDto>();
        foreach (var mo in mapObjects)
        {
            returns.Add(await mo.ToDtoAsync(gameService));
        }
        
        return Ok(returns);
    }
    
    
    [HttpGet("{mapObjectId:long}")]
    public async Task<IActionResult> GetMapObjectById(long gameId, long mapObjectId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var mapObject = await gameService.GetMapObjectById(mapObjectId);
        if (mapObject.IsError) return mapObject.GetErrorOrThrow().ToHttpResult();
        
        return Ok(await mapObject.GetValueOrThrow().ToDtoAsync(gameService));
    }

    [HttpPost("purchase/{typeId:long}")]
    public async Task<IActionResult> PurchaseMapObject(long gameId, long typeId, [FromBody] CreatePositionDto positionDto)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var type = await gameService.GetMapObjectTypeById(typeId);
        if (type.IsError) return type.GetErrorOrThrow().ToHttpResult();
        
        var position = new Position
        {
            X = positionDto.X,
            Y = positionDto.Y,
        };
        var createPositionResult = await gameService.CreatePosition(position);
        if (createPositionResult.IsSome) return createPositionResult.ToHttpResult();

        var result = await gameService.PurchaseMapObject(game.GetValueOrThrow(), type.GetValueOrThrow(), position);
        if (result.IsError) return result.ToHttpResult();

        var mapObject = result.GetValueOrThrow();
        return Ok(await mapObject.ToDtoAsync(gameService));
    }
    
    [HttpDelete("{id:long}/sell")]
    public async Task<IActionResult> SellMapObject(long gameId, long id, [FromBody] decimal refundPrice)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var mapObject = await gameService.GetMapObjectById(id);
        if (mapObject.IsError) return mapObject.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.SellMapObject(game.GetValueOrThrow(), 
            mapObject.GetValueOrThrow(), refundPrice);
        return result.ToHttpResult();
    }
    
    [HttpDelete("{id:long}/kill")]
    public async Task<IActionResult> KillMapObject(long gameId, long id)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var mapObject = await gameService.GetMapObjectById(id);
        if (mapObject.IsError) return mapObject.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.DeleteMapObject(mapObject.GetValueOrThrow());
        return result.ToHttpResult();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateMapObject(long gameId, long id, MapObjectUpdateRequest request)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var mapObject = await gameService.GetMapObjectById(id);
        if (mapObject.IsError) return mapObject.GetErrorOrThrow().ToHttpResult();

        var updated = mapObject.GetValueOrThrow();
        updated.ResourceAmount = request.ResourceAmount;
        
        var mapObjectResult = await gameService.UpdateMapObject(updated);
        if (mapObjectResult.IsSome) return mapObjectResult.ToHttpResult();

        return Ok(await updated.ToDtoAsync(gameService));
    }
}