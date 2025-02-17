using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record Animal : IKeyedDataType
{
    [Key] public long Id { get; set; }
    [MaxLength(30)] public required string Name { get; set; }
    public required long SavedGameId { get; set; }
    
    //General info
    [Range(0, 1000)] public required int Age { get; set; }
    public required AnimalSex Sex { get; set; }
    public required bool HasChip { get; set; }
    public required long AnimalTypeId { get; set; }
    public required long? PositionId { get; set; }
    
    //Nutritional info
    [Range(0, 100)] public required decimal HungerLevel { get; set; }
    [Range(0, 100)] public required decimal ThirstLevel { get; set; }
    [Range(0, 100)] public required decimal Health { get; set; }
    
    //Group information
    public long? GroupId { get; set; }

    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual AnimalType AnimalType { get; set; } = null!;
    public virtual Position? Position { get; set; }
    public virtual AnimalGroup? Group { get; set; }
}