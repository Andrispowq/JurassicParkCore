using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class JeepRoute : IKeyedDataType
{
    [Key] public long Id { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    
    public virtual ICollection<Position> RoutePositions { get; set; } = new List<Position>();
}