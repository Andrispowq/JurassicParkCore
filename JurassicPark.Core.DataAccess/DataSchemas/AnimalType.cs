using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record AnimalType : IKeyedDataType
{
    [Key] public long Id { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    public required AnimalEatingHabit EatingHabit { get; set; }
    [Range(0, Double.MaxValue)] public required decimal Price { get; set; }
    [Range(0, Double.MaxValue)] public required decimal VisitorSatisfaction { get; set; }
}