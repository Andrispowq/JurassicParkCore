using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services;

public interface ITransactionService
{
    public IEnumerable<Transaction> GetTransactionsFromLastCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    public IEnumerable<Transaction> GetAllTransactions(JurassicParkDbContext context, SavedGame savedGame);
    public Result<decimal, ServiceError> GetCurrentBalance(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Result<decimal, ServiceError>> CreateCheckpoint(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Option<ServiceError>> CreateTransaction(JurassicParkDbContext context, Transaction transaction);
}