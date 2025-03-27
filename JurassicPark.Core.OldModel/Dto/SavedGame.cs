using System;

namespace JurassicPark.Core.OldModel.Dto
{
    public class SavedGame
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public long MapWidth { get; set; }
        public long MapHeight { get; set; }
        public string MapSeed { get; set; }
        public string TimeOfDay { get; set; }
        public int DaysPassed { get; set; }
        public decimal VisitorSatisfaction { get; set; }
        public decimal HoursSinceGoalMet { get; set; }
        public GameState GameState { get; set; }
        public GameSpeed GameSpeed { get; set; }
    }

    public class SavedGameUpdateRequest
    {
        public Difficulty Difficulty { get; set; }
        public string TimeOfDay { get; set; }
        public int DaysPassed { get; set; }
        public decimal VisitorSatisfaction { get; set; }
        public decimal HoursSinceGoalMet { get; set; }
        public GameState GameState { get; set; }
        public GameSpeed GameSpeed { get; set; }
    }

    public class SavedGameCreateDto
    {
        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public long MapWidth { get; set; }
        public long MapHeight { get; set; }
    }
}