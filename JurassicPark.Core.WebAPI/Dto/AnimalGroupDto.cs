using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.WebAPI.Dto;

public class AnimalGroupDto(AnimalGroup group)
{
    public long Id => group.Id;
    public PositionDto? NextPointOfInterest => group.NextPointOfInterest is null ? null : new PositionDto(group.NextPointOfInterest);
    public AnimalTypeDto GroupType => new AnimalTypeDto(group.GroupType);
}

public static class AnimalGroupExtensions
{
    public static async Task<AnimalGroupDto> ToDtoAsync(this AnimalGroup group, IGameService gameService)
    {
        await group.LoadNavigationProperties(gameService);
        return new AnimalGroupDto(group);
    }
}