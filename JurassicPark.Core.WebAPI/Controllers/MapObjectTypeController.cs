using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;
using JurassicPark.Core.WebAPI.Dto;
using JurassicPark.Core.WebAPI.Functional;
using Microsoft.AspNetCore.Mvc;

namespace JurassicPark.Core.WebAPI.Controllers;

[ApiController]
[Route("map-object-types")]
public class MapObjectTypesController(IGameService gameService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMapObjectTypes()
    {
        var mapObjectTypes = await gameService.GetMapObjectTypes();
        return Ok(mapObjectTypes.Select(a => new MapObjectTypeDto(a)));
    }
    
    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetMapObjectTypeById(long id)
    {
        var result = await gameService.GetMapObjectTypeById(id);
        return result.ToOkResult(a => new MapObjectTypeDto(a));
    }

    [HttpPost]
    public async Task<IActionResult> AddMapObjectType([FromBody] CreateMapObjectTypeDto dto)
    {
        var type = new MapObjectType
        {
            Name = dto.Name,
            Price = dto.Price,
            ResourceAmount = dto.ResourceAmount,
            ResourceType = dto.ResourceType
        };
        
        var result = await gameService.CreateMapObjectType(type);
        if (result.IsSome)
        {
            result.ToHttpResult();
        }
        
        return CreatedAtAction(nameof(GetMapObjectTypeById), 
            new { id = type.Id }, new MapObjectTypeDto(type));
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAnimalType(long id, [FromBody] CreateMapObjectTypeDto dto)
    {
        var type = new MapObjectType
        {
            Id = id,
            Name = dto.Name,
            Price = dto.Price,
            ResourceAmount = dto.ResourceAmount,
            ResourceType = dto.ResourceType
        };
        
        var result = await gameService.UpdateMapObjectType(type);
        if (result.IsSome)
        {
            result.ToHttpResult();
        }
        
        return Ok(new MapObjectTypeDto(type));
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteMapObjectType(long id)
    {
        var type = await gameService.GetMapObjectTypeById(id);
        if (type.IsError) return type.GetErrorOrThrow().ToHttpResult();

        var result = await gameService.DeleteMapObjectType(type.GetValueOrThrow());
        return result.ToHttpResult();
    }
}