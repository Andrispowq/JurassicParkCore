using System.ComponentModel.DataAnnotations;

namespace JurassicParkCore.DataSchemas;

public class Transaction : IKeyedDataType
{
    [Key] public int Id { get; set; }
    public SavedGame SavedGame { get; set; }
    public required TransactionType Type { get; set; }
    public required decimal Amount { get; set; }
    public required bool IsCheckpoint { get; set; }
}