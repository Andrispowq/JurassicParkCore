using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class TransactionDto(Transaction transaction)
{
    public long Id => transaction.Id;
    public DateTime CreatedAt => transaction.CreatedAt;
    public TransactionType Type => transaction.Type;
    public decimal Amount => transaction.Amount;
}