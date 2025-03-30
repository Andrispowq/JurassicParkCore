using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record Position : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public long? JeepRouteId { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }
    
    public Task LoadNavigationProperties(IGameService service)
    {
        return Task.CompletedTask;
    }
}