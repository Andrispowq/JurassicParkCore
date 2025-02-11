using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class MapObject : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public required SavedGame SavedGame { get; set; }
    public required Position Position { get; set; }
    public required MapObjectType MapObjectType { get; set; }
    //This is for things like amount of water that can be drank or amount of food provided by plant
    //When purchased, equals the MapObjectType
    public required decimal ResourceAmount { get; set; }
}