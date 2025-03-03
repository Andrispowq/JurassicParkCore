using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record JeepRoute : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    
    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual ICollection<Position> RoutePositions { get; set; } = new List<Position>();
}