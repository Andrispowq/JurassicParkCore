using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface ITransactionService
{
    IEnumerable<Transaction> GetTransactionsFromLastCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    IEnumerable<Transaction> GetAllTransactions(JurassicParkDbContext context, SavedGame savedGame);
    Task<Result<Transaction, ServiceError>> GetTransactionById(JurassicParkDbContext context, long id);
    Result<decimal, ServiceError> GetCurrentBalance(JurassicParkDbContext context, SavedGame savedGame);
    Task<Result<Transaction, ServiceError>> CreateCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    Task<Option<ServiceError>> CreateTransaction(JurassicParkDbContext context, SavedGame savedGame, 
        Transaction transaction, bool allowLose = false);
}