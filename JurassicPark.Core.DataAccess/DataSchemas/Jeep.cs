using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record Jeep : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    [Range(0, 4)] public required int SeatedVisitors { get; set; }
    //null -> not deployed, else on the map
    public long? RouteId { get; set; }
    [Range(0, 100)] public decimal RouteProgression { get; set; } = decimal.Zero;
    
    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual JeepRoute? Route { get; set; }
    
    public async Task LoadNavigationProperties(IGameService service)
    {
        if (RouteId != null)
        {
            await service.LoadReference(this, o => o.Route);
            await Route!.LoadNavigationProperties(service);
        }
    }
}