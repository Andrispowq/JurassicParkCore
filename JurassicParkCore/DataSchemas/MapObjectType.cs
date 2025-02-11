using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class MapObjectType : IKeyedDataType
{
    [Key] public int Id { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    public required decimal Price { get; set; }
    public required decimal ResourceAmount { get; set; }
}