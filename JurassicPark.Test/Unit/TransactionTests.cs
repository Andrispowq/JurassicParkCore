using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Extensions;
using JurassicPark.Core.Functional;
using Transaction = JurassicPark.Core.DataSchemas.Transaction;

namespace JurassicPark.Test.Unit;

public class TransactionTests : GameRequiredTest
{
    [Test]
    public async Task TestInitialTransactionCreated()
    {
        var game = await CreateGame("asd");

        await using (var db = await GameService.CreateDbContextAsync())
        {
            var transactions = GameService.TransactionService.GetAllTransactions(db, game).ToList();
            Assert.That(transactions.Count, Is.EqualTo(1));
            var first = transactions.First();
            Assert.That(first.Type, Is.EqualTo(TransactionType.Sale));
            Assert.That(first.Amount, Is.EqualTo(Core.Services.GameService.InitialMoney));

            var currentBalanceResult = GameService.TransactionService.GetCurrentBalance(db, game);
            Assert.That(currentBalanceResult.HasValue, Is.True);

            var currentBalance = currentBalanceResult.GetValueOrThrow();
            Assert.That(currentBalance, Is.EqualTo(Core.Services.GameService.InitialMoney));
        }

        await DeleteGame(game);
    }

    [Test]
    public async Task CreateTransactionSingle()
    {
        var game = await CreateGame("asd");

        await using (var db = await GameService.CreateDbContextAsync())
        {
            var transaction = new Transaction
            {
                SavedGameId = game.Id,
                Type = TransactionType.Purchase,
                Amount = 100
            };
            
            var createResult = await GameService.TransactionService.CreateTransaction(db, game, transaction);
            Assert.That(createResult.IsNone, Is.True);
            
            var currentBalanceResult = GameService.TransactionService.GetCurrentBalance(db, game);
            Assert.That(currentBalanceResult.HasValue, Is.True);
            
            var currentBalance = currentBalanceResult.GetValueOrThrow();
            Assert.That(currentBalance, Is.EqualTo(Core.Services.GameService.InitialMoney - 100));
        }
        
        await DeleteGame(game);
    }

    [Test]
    public async Task CreateTooBigTransaction()
    {
        var game = await CreateGame("asd");

        await using (var db = await GameService.CreateDbContextAsync())
        {
            var transaction = new Transaction
            {
                SavedGameId = game.Id,
                Type = TransactionType.Purchase,
                Amount = Core.Services.GameService.InitialMoney + 1
            };
            
            //Don't create without allowing losing
            var createResult = await GameService.TransactionService.CreateTransaction(db, game, transaction);
            Assert.That(createResult.IsSome, Is.True);
            var createSome = createResult.AsSome;
            Assert.That(createSome.Value, Is.TypeOf<UnauthorizedError>());
            Assert.That(createSome.Value.Message, Is.EqualTo("Insufficient funds, transaction cancelled"));
            
            //Create with allowing losing
            var createResult2 = await GameService.TransactionService.CreateTransaction(db, game, 
                transaction, true);
            Assert.That(createResult2.IsSome, Is.True);
            var createSome2 = createResult2.AsSome;
            Assert.That(createSome2.Value, Is.TypeOf<UnauthorizedError>());
            Assert.That(createSome2.Value.Message, Is.EqualTo("Insufficient funds, game lost"));

            Assert.That(game.GameState, Is.EqualTo(GameState.Lost));
        }
        
        await DeleteGame(game);
    }

    private async Task<Transaction> CreateTransaction(SavedGame game, decimal amount)
    {
        await using var db = await GameService.CreateDbContextAsync();
        
        var transaction = new Transaction
        {
            SavedGameId = game.Id,
            Type = TransactionType.Purchase,
            Amount = amount
        };
            
        var createResult = await GameService.TransactionService.CreateTransaction(db, game, transaction);
        Assert.That(createResult.IsNone, Is.True);
        
        return transaction;
    }

    [Test]
    public async Task CreateCheckpoint()
    {
        var game = await CreateGame("asd1");

        //Create a couple transactions
        var totalRemoved = 0;
        foreach (int i in ..10)
        {
            var amount = 100 * i;
            totalRemoved += amount;
            _ = await CreateTransaction(game, amount);
        }
        
        var balance = Core.Services.GameService.InitialMoney - totalRemoved;
        
        await using var db = await GameService.CreateDbContextAsync();
        
        //Check the results
        var currentBalanceResult = GameService.TransactionService.GetCurrentBalance(db, game);
        Assert.That(currentBalanceResult.HasValue, Is.True);
        Assert.That(currentBalanceResult.GetValueOrThrow(), Is.EqualTo(balance));
        
        //Make a checkpoint
        var checkpointResult = await GameService.TransactionService.CreateCheckpoint(db, game);
        Assert.That(checkpointResult.HasValue, Is.True);
        var checkpoint = checkpointResult.GetValueOrThrow();
        Assert.That(checkpoint.Type, Is.EqualTo(TransactionType.Checkpoint));
        Assert.That(checkpoint.Amount, Is.EqualTo(balance));

        //Check if we only got the checkpoint back
        var transactions = GameService.TransactionService.GetTransactionsFromLastCheckpoint(db, game);
        transactions = transactions.ToList();
        Assert.That(transactions, Is.Not.Null);
        Assert.That(transactions.Count(), Is.EqualTo(1));
        Assert.That(transactions, Is.EquivalentTo(new [] { checkpoint }));
        
        //Check if the balance is still the same
        var currentBalance2Result = GameService.TransactionService.GetCurrentBalance(db, game);
        Assert.That(currentBalance2Result.HasValue, Is.True);
        Assert.That(currentBalance2Result.GetValueOrThrow(), Is.EqualTo(balance));
    }

    [Test]
    public async Task CreateCheckpointAndTestUsage()
    {
        var game = await CreateGame("asd1");

        //Create a couple transactions
        var totalRemoved = 0;
        foreach (int i in ..10)
        {
            var amount = 100 * i;
            totalRemoved += amount;
            _ = await CreateTransaction(game, amount);
        }
        
        var balance = Core.Services.GameService.InitialMoney - totalRemoved;

        Transaction checkpoint;
        await using (var db = await GameService.CreateDbContextAsync())
        {
            //Check the results
            var currentBalanceResult = GameService.TransactionService.GetCurrentBalance(db, game);
            Assert.That(currentBalanceResult.HasValue, Is.True);
            Assert.That(currentBalanceResult.GetValueOrThrow(), Is.EqualTo(balance));

            //Make a checkpoint
            var checkpointResult = await GameService.TransactionService.CreateCheckpoint(db, game);
            Assert.That(checkpointResult.HasValue, Is.True);
            checkpoint = checkpointResult.GetValueOrThrow();
            Assert.That(checkpoint.Type, Is.EqualTo(TransactionType.Checkpoint));
            Assert.That(checkpoint.Amount, Is.EqualTo(balance));

            //Check if we only got the checkpoint back
            var transactions = GameService.TransactionService.GetTransactionsFromLastCheckpoint(db, game);
            transactions = transactions.ToList();
            Assert.That(transactions, Is.Not.Null);
            Assert.That(transactions.Count(), Is.EqualTo(1));
            Assert.That(transactions, Is.EquivalentTo(new[] { checkpoint }));

            //Check if the balance is still the same
            var currentBalance2Result = GameService.TransactionService.GetCurrentBalance(db, game);
            Assert.That(currentBalance2Result.HasValue, Is.True);
            Assert.That(currentBalance2Result.GetValueOrThrow(), Is.EqualTo(balance));
        }

        foreach (int i in ..10)
        {
            var amount = 100 * i;
            totalRemoved += amount;
            _ = await CreateTransaction(game, amount);
        }
        
        balance = Core.Services.GameService.InitialMoney - totalRemoved;
        
        await using (var db = await GameService.CreateDbContextAsync())
        {
            //Check the results
            var currentBalanceResult = GameService.TransactionService.GetCurrentBalance(db, game);
            Assert.That(currentBalanceResult.HasValue, Is.True);
            Assert.That(currentBalanceResult.GetValueOrThrow(), Is.EqualTo(balance));

            //Check if we only got the checkpoint back
            var transactions = GameService.TransactionService.GetTransactionsFromLastCheckpoint(db, game);
            transactions = transactions.ToList();
            Assert.That(transactions, Is.Not.Null);
            Assert.That(transactions.Count(), Is.EqualTo(12));
            Assert.That(transactions.First(), Is.EqualTo(checkpoint));

            var i = 0;
            foreach (var transaction in transactions.Skip(1))
            {
                Assert.That(transaction.Type, Is.EqualTo(TransactionType.Purchase));
                Assert.That(transaction.Amount, Is.EqualTo(i * 100));
                i++;
            }
        }
    }
}