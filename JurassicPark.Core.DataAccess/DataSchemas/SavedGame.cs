using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record SavedGame : ITimestamped, IKeyedDataType
{
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    [MaxLength(100)] public required string Name { get; init; }
    public required Difficulty Difficulty { get; set; }
    [Range(0, long.MaxValue)] public required long MapWidth { get; init; }
    [Range(0, long.MaxValue)] public required long MapHeight { get; init; }
    [MaxLength(200)] public required string MapSeed { get; init; }
    public required TimeOnly TimeOfDay { get; set; }
    [Range(0, Int32.MaxValue)] public required int DaysPassed { get; set; }
    [Range(0, 100)] public required decimal VisitorSatisfaction { get; set; }
    [Range(0, Double.MaxValue)] public required decimal HoursSinceGoalMet { get; set; }
    public required GameState GameState { get; set; }
    public required GameSpeed GameSpeed { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = [];
    public virtual ICollection<AnimalGroup> AnimalGroups { get; set; } = [];
    public virtual ICollection<Jeep> Jeeps { get; set; } = [];
    public virtual ICollection<MapObject> MapObjects { get; set; } = [];
    public virtual ICollection<Transaction> Transactions { get; set; } = [];
    
    public async Task LoadNavigationProperties(IGameService service)
    {
        await service.LoadCollection(this, o => o.Animals);
        await service.LoadCollection(this, o => o.AnimalGroups);
        await service.LoadCollection(this, o => o.Jeeps);
        await service.LoadCollection(this, o => o.MapObjects);
        }
}