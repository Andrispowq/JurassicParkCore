using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class SavedGame : ITimestamped, IKeyedDataType
{
    [Key] public int Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required Difficulty Difficulty { get; set; }
    public required Position MapSize { get; set; }
    public required string MapSeed { get; set; }
    public required TimeOnly TimeOfDay { get; set; }
    public required int DaysPassed { get; set; }
    public required GameSpeed GameSpeed { get; set; }
    
    public List<Animal> Animals { get; set; } = new();
    public List<AnimalGroup> AnimalGroups { get; set; } = new();
    public List<Jeep> Jeeps { get; set; } = new();
    public List<MapObject> MapObjects { get; set; } = new();
    public List<Transaction> Transactions { get; set; } = new();
}