using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class AnimalGroupDto(AnimalGroup group)
{
    public long Id => group.Id;
    public PositionDto? NextPointOfInterest => group.NextPointOfInterest is null ? null : new PositionDto(group.NextPointOfInterest);
    public AnimalTypeDto GroupType => new AnimalTypeDto(group.GroupType);
}