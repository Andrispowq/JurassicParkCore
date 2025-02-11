using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class MapObject : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public SavedGame SavedGame { get; set; }
}