using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public record Jeep : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    public required int SeatedVisitors { get; set; }
    //null -> not deployed, else on the map
    public long? RouteId { get; set; }
    public decimal RouteProgression { get; set; } = decimal.Zero;
    
    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual JeepRoute? Route { get; set; }
}