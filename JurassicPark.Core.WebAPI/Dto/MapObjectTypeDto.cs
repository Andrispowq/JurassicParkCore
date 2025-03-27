using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class MapObjectTypeDto(MapObjectType mapObjectType)
{
    public long Id => mapObjectType.Id;
    public string Name => mapObjectType.Name;
    public ResourceType ResourceType => mapObjectType.ResourceType;
    public decimal ResourceAmount => mapObjectType.ResourceAmount;
    public decimal Price => mapObjectType.Price;
}

public class CreateMapObjectTypeDto
{
    public required string Name { get; init; }
    public ResourceType ResourceType { get; init; }
    public decimal ResourceAmount { get; init; }
    public decimal Price { get; init; }
}