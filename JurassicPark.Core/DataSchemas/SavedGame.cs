using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public record SavedGame : ITimestamped, IKeyedDataType
{
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required string Name { get; set; }
    public required Difficulty Difficulty { get; set; }
    public required long MapWidth { get; set; }
    public required long MapHeight { get; set; }
    public required string MapSeed { get; set; }
    public required TimeOnly TimeOfDay { get; set; }
    public required int DaysPassed { get; set; }
    public required decimal VisitorSatisfaction { get; set; }
    public required decimal HoursSinceGoalMet { get; set; }
    public required GameState GameState { get; set; }
    public required GameSpeed GameSpeed { get; set; }
    
    public List<Animal> Animals { get; set; } = new();
    public List<AnimalGroup> AnimalGroups { get; set; } = new();
    public List<Jeep> Jeeps { get; set; } = new();
    public List<MapObject> MapObjects { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}