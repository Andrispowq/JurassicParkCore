using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Animal : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    
    //General info
    public required long AnimalTypeId { get; set; }
    public required long? PositionId { get; set; }
    
    //Nutritional info
    public required decimal HungerLevel { get; set; }
    public required decimal ThirstLevel { get; set; }
    public required decimal Health { get; set; }
    
    //Group information
    public long? GroupId { get; set; }
    
    public virtual SavedGame SavedGame { get; set; }
    public virtual AnimalType AnimalType { get; set; }
    public virtual Position? Position { get; set; }
    public virtual AnimalGroup? Group { get; set; }
}