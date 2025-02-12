using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Transaction : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    public required TransactionType Type { get; set; }
    public required decimal Amount { get; set; }
    
    public virtual SavedGame SavedGame { get; set; }
}