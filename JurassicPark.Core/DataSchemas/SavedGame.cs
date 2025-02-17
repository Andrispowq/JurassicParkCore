using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record SavedGame : ITimestamped, IKeyedDataType
{
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    [MaxLength(100)] public required string Name { get; set; }
    public required Difficulty Difficulty { get; set; }
    [Range(0, long.MaxValue)] public required long MapWidth { get; set; }
    [Range(0, long.MaxValue)] public required long MapHeight { get; set; }
    [MaxLength(200)] public required string MapSeed { get; set; }
    public required TimeOnly TimeOfDay { get; set; }
    [Range(0, Int32.MaxValue)] public required int DaysPassed { get; set; }
    [Range(0, 100)] public required decimal VisitorSatisfaction { get; set; }
    [Range(0, Double.MaxValue)] public required decimal HoursSinceGoalMet { get; set; }
    public required GameState GameState { get; set; }
    public required GameSpeed GameSpeed { get; set; }
    
    public List<Animal> Animals { get; set; } = new();
    public List<AnimalGroup> AnimalGroups { get; set; } = new();
    public List<Jeep> Jeeps { get; set; } = new();
    public List<MapObject> MapObjects { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}