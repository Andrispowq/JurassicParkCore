using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public record Transaction : IKeyedDataType, ITimestamped
{
    [Key] public long Id { get; set; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public required long SavedGameId { get; set; }
    public required TransactionType Type { get; set; }
    public required decimal Amount { get; set; }
    
    public virtual SavedGame SavedGame { get; set; }
}