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
        List<AnimalDto> returns = new List<AnimalDto>();
        foreach (var animal in animals)
        {
            returns.Add(await animal.ToDtoAsync(gameService));
        }
        
        return Ok(returns);
    }
    
    [HttpGet("{animalId:long}")]
    public async Task<IActionResult> GetAnimalById(long gameId, long animalId)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var animal = await gameService.GetAnimalById(animalId);
        if (animal.IsError) return animal.GetErrorOrThrow().ToHttpResult();
        
        return Ok(await animal.GetValueOrThrow().ToDtoAsync(gameService));
    }
    
    [HttpPost("purchase/{typeId:long}")]
    public async Task<IActionResult> PurchaseAnimal(long gameId, long typeId, [FromBody] CreatePositionDto positionDto)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        
        var type = await gameService.GetAnimalTypeById(typeId);
        if (type.IsError) return type.GetErrorOrThrow().ToHttpResult();

        var position = new Position
        {
            X = positionDto.X,
            Y = positionDto.Y,
        };
        var createPositionResult = await gameService.CreatePosition(position);
        if (createPositionResult.IsSome) return createPositionResult.ToHttpResult();
        
        var result = await gameService.PurchaseAnimal(game.GetValueOrThrow(), type.GetValueOrThrow(), position);
        if (result.IsError) return result.ToHttpResult();

        var animal = result.GetValueOrThrow();
        return Ok(await animal.ToDtoAsync(gameService));
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

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAnimal(long gameId, long id, AnimalUpdateRequest request)
    {
        var game = await gameService.GetSavedGame(gameId);
        if (game.IsError) return game.GetErrorOrThrow().ToHttpResult();
        var animal = await gameService.GetAnimalById(id);
        if (animal.IsError) return animal.GetErrorOrThrow().ToHttpResult();

        var updated = animal.GetValueOrThrow();
        updated.Age = request.Age;
        updated.HasChip = request.HasChip;
        updated.State = request.State;
        updated.HungerLevel = request.HungerLevel;
        updated.ThirstLevel = request.ThirstLevel;
        updated.Health = request.Health;

        await gameService.LoadReference(updated, a => a.Position);
        var position = updated.Position;
        position.X = request.Position.X;
        position.Y = request.Position.Y;

        var result = await gameService.UpdatePosition(position);
        if (result.IsSome) return result.ToHttpResult();

        var animalResult = await gameService.UpdateAnimal(updated);
        if (animalResult.IsSome) return animalResult.ToHttpResult();

        return Ok(await updated.ToDtoAsync(gameService));
    }
}