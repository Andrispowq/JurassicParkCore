using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.WebAPI.Dto;

public class AnimalDto(Animal animal)
{
    public long Id => animal.Id;
    public string Name => animal.Name;
    
    public int Age => animal.Age;
    public AnimalSex Sex => animal.Sex;
    public bool HasChip => animal.HasChip;
    public AnimalState State => animal.State;
    
    public decimal HungerLevel => animal.HungerLevel;
    public decimal ThirstLevel => animal.ThirstLevel;
    public decimal Health => animal.Health;

    public AnimalTypeDto AnimalType => new(animal.AnimalType);
    public PositionDto Position => new(animal.Position);
    public PositionDto? PointOfInterest => animal.PointOfInterest is null ? null : new(animal.PointOfInterest);
    public AnimalGroupDto? Group => animal.Group is null ? null : new(animal.Group);
    
    public List<MapObjectDto> DiscoveredMapObjects => animal.DiscoveredMapObjects
        .Select(d => new MapObjectDto(d.MapObject))
        .ToList();
}

public class AnimalUpdateRequest
{
    public required int Age { get; init; }
    public required bool HasChip { get; init; }
    public required AnimalState State { get; init; }
    
    public required decimal HungerLevel { get; init; }
    public required decimal ThirstLevel { get; init; }
    public required decimal Health { get; init; }

    public required CreatePositionDto Position { get; init; }
}

public static class AnimalExtensions
{
    public static async Task<AnimalDto> ToDtoAsync(this Animal animal, IGameService service)
    {
        await animal.LoadNavigationProperties(service);
        return new AnimalDto(animal);
    }
}