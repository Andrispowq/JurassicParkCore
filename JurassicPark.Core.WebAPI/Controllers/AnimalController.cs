using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("games/{gameId:long}/animals")]
public class AnimalController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAnimals(long gameId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var animals = await gameService.GetAnimals(game.GetValueOrThrow());
        return Ok(animalDtos);
    }
    
    [HttpPost("purchase/{typeId:long}")]
    public async Task<IActionResult> PurchaseAnimal(long gameId, long typeId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var type = await gameService.GetAnimalTypeById(typeId);
        if (type.IsError) return type.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.PurchaseAnimal(game.GetValueOrThrow(), type.GetValueOrThrow());
        return result.ToOkResult(a => new AnimalDto(a));
    }
    
    [HttpDelete("{id:long}/sell")]
    public async Task<IActionResult> SellAnimal(long gameId, long id, [FromQuery] decimal refundPrice)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var animal = await gameService.GetAnimalById(id);
        if (animal.IsError) return animal.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.SellAnimal(game.GetValueOrThrow(), 
            animal.GetValueOrThrow(), refundPrice);
        return result.ToHttpResult();
    }
    
    [HttpDelete("{id:long}/kill")]
    public async Task<IActionResult> KillAnimal(long gameId, long id)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var animal = await gameService.GetAnimalById(id);
        if (animal.IsError) return animal.GetErrorOrThrow().ToHttpResult();
        
        var result = await gameService.DeleteAnimal(animal.GetValueOrThrow());
        return result.ToHttpResult();
    }
}