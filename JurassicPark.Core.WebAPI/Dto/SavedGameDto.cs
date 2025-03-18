using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class SavedGameDto(SavedGame savedGame)
{
    public long Id { get; } = savedGame.Id;
    public DateTime CreatedAt { get; } = savedGame.CreatedAt;
    public string Name { get; } = savedGame.Name;
    public Difficulty Difficulty { get; } = savedGame.Difficulty;
    public long MapWidth { get; } = savedGame.MapWidth;
    public long MapHeight { get; } = savedGame.MapHeight;
    public string MapSeed { get; } = savedGame.MapSeed;
    public TimeOnly TimeOfDay { get; } = savedGame.TimeOfDay;
    public int DaysPassed { get; } = savedGame.DaysPassed;
    public decimal VisitorSatisfaction { get; } = savedGame.VisitorSatisfaction;
    public decimal HoursSinceGoalMet { get; } = savedGame.HoursSinceGoalMet;
    public GameState GameState { get; } = savedGame.GameState;
    public GameSpeed GameSpeed { get; } = savedGame.GameSpeed;
}

public class SavedGameUpdateRequest
{
    public required Difficulty Difficulty { get; init; }
    public required TimeOnly TimeOfDay { get; init; }
    public required int DaysPassed { get; init; }
    public required decimal VisitorSatisfaction { get; init; }
    public required decimal HoursSinceGoalMet { get; init; }
    public required GameState GameState { get; init; }
    public required GameSpeed GameSpeed { get; init; }
}

public class SavedGameCreateDto
{
    [MaxLength(50)] public required string Name { get; init; }
    public required Difficulty Difficulty { get; init; }
    public required long MapWidth { get; init; }
    public required long MapHeight { get; init; }
}