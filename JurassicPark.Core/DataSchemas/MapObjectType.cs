using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record MapObjectType : IKeyedDataType
{
    [Key] public long Id { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    [Range(0, Double.MaxValue)] public required decimal Price { get; set; }
    public required ResourceType ResourceType { get; set; }
    [Range(0, Double.MaxValue)] public required decimal ResourceAmount { get; set; }
}