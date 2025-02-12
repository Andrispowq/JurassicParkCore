using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class AnimalGroup : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    public required long? PositionId { get; set; }
    public required long GroupTypeId { get; set; }
    public virtual SavedGame SavedGame { get; set; }
    public virtual Position? NextPointOfInterest { get; set; }
    public virtual AnimalType GroupType { get; set; }
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
}