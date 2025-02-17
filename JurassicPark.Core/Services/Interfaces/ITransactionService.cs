using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface ITransactionService
{
    public IEnumerable<Transaction> GetTransactionsFromLastCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    public IEnumerable<Transaction> GetAllTransactions(JurassicParkDbContext context, SavedGame savedGame);
    public Result<decimal, ServiceError> GetCurrentBalance(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Result<decimal, ServiceError>> CreateCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Option<ServiceError>> CreateTransaction(JurassicParkDbContext context, SavedGame savedGame, 
        Transaction transaction, bool allowLose = false);
}