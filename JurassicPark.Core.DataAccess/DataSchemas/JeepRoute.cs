using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record JeepRoute : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    
    public virtual SavedGame SavedGame { get; set; } = null!;
    public virtual ICollection<Position> RoutePositions { get; set; } = new List<Position>();
    
    public async Task LoadNavigationProperties(IGameService service)
    {
        await service.LoadCollection(this, o => o.RoutePositions);
    }
}