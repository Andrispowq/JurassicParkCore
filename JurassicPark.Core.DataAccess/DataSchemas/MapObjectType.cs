using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record MapObjectType : IKeyedDataType
{
    [Key] public long Id { get; set; }
    [MaxLength(20)] public required string Name { get; set; }
    [Range(0, Double.MaxValue)] public required decimal Price { get; set; }
    public required ResourceType ResourceType { get; set; }
    [Range(0, Double.MaxValue)] public required decimal ResourceAmount { get; set; }
    
    public Task LoadNavigationProperties(IGameService service)
    {
        return Task.CompletedTask;
    }
}