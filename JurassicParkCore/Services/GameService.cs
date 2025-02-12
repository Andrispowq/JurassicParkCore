using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;
using Microsoft.EntityFrameworkCore;

namespace JurassicParkCore.Services;

public class GameService(
    IDbContextFactory<JurassicParkDbContext> dbContextFactory,
    IAnimalService animalService,
    IPositionService positionService,
    ITransactionService transactionService) : IGameService
{
    public IAnimalService AnimalService => animalService;
    public IPositionService PositionService => positionService;
    public ITransactionService TransactionService => transactionService;
    
    private const decimal InitialMoney = 100000;

    public async Task<JurassicParkDbContext> GetDbContextAsync()
    {
        return await dbContextFactory.CreateDbContextAsync();
    }
    
    public async Task<Result<SavedGame, ServiceError>> CreateNewGame(Difficulty difficulty, long width, long height)
    {
        var game = new SavedGame
        {
            Difficulty = difficulty,
            MapWidth = width,
            MapHeight = height,
            MapSeed = Utils.GenerateEncryptionKey(),
            TimeOfDay = new TimeOnly(9, 0, 0), //Start at 9:00 AM
            DaysPassed = 0,
            VisitorSatisfaction = 0,
            HoursSinceGoalMet = 0,
            GameState = GameState.Ongoing,
            GameSpeed = GameSpeed.Moderate
        };
        
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.SavedGames.Create(game);
        if (result is Option<DatabaseError>.Some some)
        {
            return new ConflictError(some.Value.Message);
        }

        var initialTransaction = new Transaction()
        {
            Type = TransactionType.Sale,
            Amount = InitialMoney,
            SavedGameId = game.Id
        };
        
        var transactionResult = await transactionService.CreateTransaction(db, initialTransaction);
        return transactionResult.Map<Result<SavedGame, ServiceError>>(
            error => new ConflictError(error.Message),
            () => game);
    }

    public async Task<Result<Animal, ServiceError>> PurchaseAnimal(SavedGame game, AnimalType type)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var animal = new Animal()
            {
                AnimalTypeId = type.Id,
                Group = null,
                Health = 100,
                HungerLevel = 0,
                ThirstLevel = 0,
                PositionId = null,
                SavedGameId = game.Id
            };

            var animalResult = await animalService.CreateAnimal(db, animal);
            if (animalResult is Option<ServiceError>.Some animalError)
            {
                await transaction.RollbackAsync();
                return new ConflictError(animalError.Value.Message);
            }

            var purchase = new Transaction()
            {
                Type = TransactionType.Purchase,
                Amount = type.Price,
                SavedGameId = game.Id
            };
            
            var transactionResult = await transactionService.CreateTransaction(db, purchase);
            if (transactionResult is Option<ServiceError>.Some transactionError)
            {
                await transaction.RollbackAsync();
                return new ConflictError(transactionError.Value.Message);
            }
            
            await transaction.CommitAsync();
            return Result<Animal, ServiceError>.Success(animal);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new BadRequestError(ex.Message);
        }
    }
}