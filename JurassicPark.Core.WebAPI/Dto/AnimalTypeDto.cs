using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class AnimalTypeDto(AnimalType animalType)
{
    public long Id => animalType.Id;
    public string Name => animalType.Name;
    public AnimalEatingHabit EatingHabit => animalType.EatingHabit;
    public decimal Price => animalType.Price;
    public decimal VisitorSatisfaction => animalType.VisitorSatisfaction;
}

public class CreateAnimalTypeRequest
{
    public required string Name { get; init; }
    public required AnimalEatingHabit EatingHabit { get; init; }
    public required decimal Price { get; init; }
    public required decimal VisitorSatisfaction { get; init; }
}