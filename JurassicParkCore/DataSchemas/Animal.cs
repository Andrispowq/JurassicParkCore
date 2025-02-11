using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Animal : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public required SavedGame SavedGame { get; set; }
    
    //General info
    public required AnimalType AnimalType { get; set; }
    public required Position? Position { get; set; }
    
    //Nutritional info
    public required decimal HungerLevel { get; set; }
    public required decimal ThirstLevel { get; set; }
    public required decimal Health { get; set; }
    
    //Group information
    public AnimalGroup? Group { get; set; }
}