using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services;

public class TransactionService : ITransactionService
{
    public IEnumerable<Transaction> GetTransactionsFromLastCheckpoint(JurassicParkDbContext context, SavedGame savedGame)
    {
        var transactions = context.Transactions.All.Where(t => t.SavedGame.Id == savedGame.Id);
        var lastCheckpoint = transactions.LastOrDefault(t => t.Type == TransactionType.Checkpoint);
        if (lastCheckpoint is null)
        {
            //No checkpoint yet, return all
            return transactions; 
        }

        //Return all those that are after the checkpoint (including it)
        return transactions.SkipWhile(t => t.Id < lastCheckpoint.Id);
    }

    public IEnumerable<Transaction> GetAllTransactions(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.Transactions.All.Where(t => t.SavedGame.Id == savedGame.Id);
    }

    public Result<decimal, ServiceError> GetCurrentBalance(JurassicParkDbContext context, SavedGame savedGame)
    {
        var fromLastCheckpoint = GetTransactionsFromLastCheckpoint(context, savedGame).ToList();
        var purchaseSum = fromLastCheckpoint.Where(t => t.Type == TransactionType.Purchase)
            .Sum(t => t.Amount);
        var saleSum = fromLastCheckpoint.Where(t => t.Type == TransactionType.Sale)
            .Sum(t => t.Amount);
        return saleSum - purchaseSum;
    }

    public async Task<Result<decimal, ServiceError>> CreateCheckpoint(JurassicParkDbContext context, SavedGame savedGame)
    {
        var currentBalance = GetCurrentBalance(context, savedGame);
        if (currentBalance.IsError)
        {
            return new BadRequestError(currentBalance.GetErrorOrThrow().ToString() ?? "");
        }

        var balance = currentBalance.GetValueOrThrow();
        var checkpoint = new Transaction()
        {
            Type = TransactionType.Checkpoint,
            Amount = balance,
            SavedGameId = savedGame.Id
        };
        
        var createResult = await context.Transactions.Create(checkpoint);
        return createResult.Map<Result<decimal, ServiceError>>(
            error => new ConflictError(error.Message),
            () => balance);
    }

    public async Task<Option<ServiceError>> CreateTransaction(JurassicParkDbContext context, Transaction transaction)
    {
        var createResult = await context.Transactions.Create(transaction);
        return createResult.MapOption<ServiceError>(_ => new ConflictError("Transaction already exists"));
    }
}