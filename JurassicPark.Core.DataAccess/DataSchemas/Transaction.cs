using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record Transaction : IKeyedDataType, ITimestamped
{
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required long SavedGameId { get; set; }
    public required TransactionType Type { get; set; }
    [Range(0, Double.MaxValue)] public required decimal Amount { get; set; }

    public virtual SavedGame SavedGame { get; set; } = null!;

    public Task LoadNavigationProperties(IGameService service)
    {
        return Task.CompletedTask;
    }
}