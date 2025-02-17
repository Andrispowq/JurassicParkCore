using JurassicPark.Core.DataSchemas;
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
}