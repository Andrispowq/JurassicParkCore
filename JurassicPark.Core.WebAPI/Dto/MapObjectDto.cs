using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.WebAPI.Dto;

public class MapObjectDto(MapObject mapObject)
{
    public long Id => mapObject.Id;
    public decimal ResourceAmount => mapObject.ResourceAmount;
    
    public MapObjectTypeDto MapObjectType => new(mapObject.MapObjectType);
    public PositionDto Position => new(mapObject.Position);
    
    public List<AnimalDto> DiscoveredBy => mapObject.DiscoveredByAnimals
        .Select(d => new AnimalDto(d.Animal))
        .ToList();
}

public class MapObjectUpdateRequest
{
    public required decimal ResourceAmount { get; init; }
}

public static class MapObjectExtensions
{
    public static async Task<MapObjectDto> ToDtoAsync(this MapObject mapObject, IGameService service)
    {
        await mapObject.LoadNavigationProperties(service);
        return new MapObjectDto(mapObject);
    }
}