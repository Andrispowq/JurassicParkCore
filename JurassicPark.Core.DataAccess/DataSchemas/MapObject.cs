using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record MapObject : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    //Null -> not yet placed
    public required long? PositionId { get; set; }
    public required long MapObjectTypeId { get; set; }
    //This is for things like amount of water that can be drank or amount of food provided by plant
    //When purchased, equals the MapObjectType
    [Range(0, Double.MaxValue)] public required decimal ResourceAmount { get; set; }
    
    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual Position Position { get; set; } = null!;
    public virtual MapObjectType MapObjectType { get; set; } = null!;

    public virtual ICollection<Discovered> DiscoveredByAnimals { get; set; } = [];
}