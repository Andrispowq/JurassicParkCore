using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Position : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public long? JeepRouteId { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }
}