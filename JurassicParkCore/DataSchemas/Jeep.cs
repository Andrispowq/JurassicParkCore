using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Jeep : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public SavedGame SavedGame { get; set; }
    public required int SeatedVisitors { get; set; }
    //null -> not deployed, else on the map
    public required Position? Position { get; set; }
}