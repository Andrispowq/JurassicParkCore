using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Position : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }
}