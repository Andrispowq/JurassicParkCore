using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class AnimalGroup : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public SavedGame SavedGame { get; set; }
    public required Position? NextPointOfInterest { get; set; }
    public required AnimalType GroupType { get; set; }
    public List<Animal> Animals { get; set; } = new();
}