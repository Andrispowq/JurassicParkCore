using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("animal_types")]
public class AnimalTypeController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAnimalTypes()
    {
        var animalTypes = await gameService.GetAnimalTypes();
        return Ok(animalTypes.Select(a => new AnimalTypeDto(a)));
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetAnimalTypeById(long id)
    {
        var result = await gameService.GetAnimalTypeById(id);
        return result.ToOkResult(a => new AnimalTypeDto(a));
    }

    [HttpPost]
    public async Task<IActionResult> AddAnimalType([FromBody] CreateAnimalTypeRequest dto)
    {
        var animalType = new AnimalType
        {
            Name = dto.Name,
            Price = dto.Price,
            EatingHabit = dto.EatingHabit,
            VisitorSatisfaction = dto.VisitorSatisfaction,
        };
        
        var result = await gameService.CreateAnimalType(animalType);
        if (result.IsSome)
        {
            result.ToHttpResult();
        }
        
        return CreatedAtAction(nameof(GetAnimalTypeById), 
            new { id = animalType.Id }, new AnimalTypeDto(animalType));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAnimalType(long id, [FromBody] CreateAnimalTypeRequest dto)
    {
        var animalType = new AnimalType
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price,
            EatingHabit = dto.EatingHabit,
            VisitorSatisfaction = dto.VisitorSatisfaction,
        };
        
        var result = await gameService.UpdateAnimalType(animalType);
        if (result.IsSome)
        {
            result.ToHttpResult();
        }
        
        return Ok(new AnimalTypeDto(animalType));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAnimalType(long id)
    {
        var type = await gameService.GetAnimalTypeById(id);
        if (type.IsError) return type.GetErrorOrThrow().ToHttpResult();

        var result = await gameService.DeleteAnimalType(type.GetValueOrThrow());
        return result.ToHttpResult();
    }
}