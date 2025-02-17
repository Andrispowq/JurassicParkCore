using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;
using JurassicParkCore.Services.Interfaces;

namespace JurassicParkCore.Services;

public class TransactionService : ITransactionService
{
    public IEnumerable<Transaction> GetTransactionsFromLastCheckpoint(JurassicParkDbContext context, SavedGame savedGame)
    {
        var transactions = context.Transactions.All
            .Where(t => t.SavedGame.Id == savedGame.Id)
            .OrderBy(t => t.CreatedAt);
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
        decimal startingAmount = 0;
        if (fromLastCheckpoint.Any())
        {
            var first = fromLastCheckpoint.First();
            if (first.Type == TransactionType.Checkpoint)
            {
                startingAmount = first.Amount;
            }
        }
        
        var purchaseSum = fromLastCheckpoint.Where(t => t.Type == TransactionType.Purchase)
            .Sum(t => t.Amount);
        var saleSum = fromLastCheckpoint.Where(t => t.Type == TransactionType.Sale)
            .Sum(t => t.Amount);
        return startingAmount + saleSum - purchaseSum;
    }

    public async Task<Result<decimal, ServiceError>> CreateCheckpoint(JurassicParkDbContext context, SavedGame savedGame)
    {
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

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

    public async Task<Option<ServiceError>> CreateTransaction(JurassicParkDbContext context, SavedGame savedGame, Transaction transaction)
    {
        if (savedGame.Id != transaction.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var createResult = await context.Transactions.Create(transaction);
        if (createResult is Option<DatabaseError>.Some error)
        {
            return new ConflictError(error.Value.Message);
        }
        
        var balanceResult = GetCurrentBalance(context, savedGame);
        if (balanceResult.IsError)
        {
            return new BadRequestError(balanceResult.GetErrorOrThrow().Message);
        }
        
        var balance = balanceResult.GetValueOrThrow();
        if (balance >= decimal.Zero) return new Option<ServiceError>.None();
        
        savedGame.GameState = GameState.Lost;
        await context.SavedGames.Update(savedGame);
        return new UnauthorizedError("Insufficient funds, game lost");
    }
}