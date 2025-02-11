using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class AnimalType : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public required AnimalEatingHabit EatingHabit { get; set; }
    public required decimal Price { get; set; }
    public required decimal VisitorSatisfaction { get; set; }
}