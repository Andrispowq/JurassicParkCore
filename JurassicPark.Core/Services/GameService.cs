using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Core.Services;

public class GameService(
    IDbContextFactory<JurassicParkDbContext> dbContextFactory,
    IAnimalService animalService,
    IJeepService jeepService,
    IMapObjectService mapObjectService,
    IPositionService positionService,
    ITransactionService transactionService) : IGameService
{
    public IAnimalService AnimalService => animalService;
    public IJeepService JeepService => jeepService;
    public IMapObjectService MapObjectService => mapObjectService;
    public IPositionService PositionService => positionService;
    public ITransactionService TransactionService => transactionService;
    
    public const decimal InitialMoney = 100000;
    public static readonly TimeOnly StartTime = new(9, 0, 0);

    public async Task<JurassicParkDbContext> CreateDbContextAsync()
    {
        return await dbContextFactory.CreateDbContextAsync();
    }

    public async Task<Option<ServiceError>> InitialiseComponents(
        List<AnimalType> animalTypes,
        List<MapObjectType> mapObjectTypes)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            foreach (var animal in animalTypes)
            {
                var withName = db.AnimalTypes.All.FirstOrDefault(x => x.Name == animal.Name);
                if (withName is null)
                {
                    await db.AnimalTypes.Create(animal);
                }
                else
                {
                    animal.Id = withName.Id;
                }
            }
            
            foreach (var mapObject in mapObjectTypes)
            {
                var withName = db.MapObjectTypes.All.FirstOrDefault(x => x.Name == mapObject.Name);
                if (withName is null)
                {
                    await db.MapObjectTypes.Create(mapObject);
                }
                else
                {
                    mapObject.Id = withName.Id;
                }
            }
            
            await transaction.CommitAsync();
            return new Option<ServiceError>.None();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ConflictError(ex.Message);
        }
    }

    public async Task<bool> PruneDatabase(SavedGame? savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            //If the game is not specified, clear everything
            if (savedGame is null)
            {
                await db.Animals.All.ExecuteDeleteAsync();
                await db.AnimalTypes.All.ExecuteDeleteAsync();
                await db.AnimalGroups.All.ExecuteDeleteAsync();
                await db.Jeeps.All.ExecuteDeleteAsync();
                await db.JeepRoutes.All.ExecuteDeleteAsync();
                await db.MapObjects.All.ExecuteDeleteAsync();
                await db.MapObjectTypes.All.ExecuteDeleteAsync();
                await db.Positions.All.ExecuteDeleteAsync();
                await db.SavedGames.All.ExecuteDeleteAsync();
                await db.Transactions.All.ExecuteDeleteAsync();
            }
            else
            {
                await db.Animals.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
                await db.AnimalGroups.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
                await db.Jeeps.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
                await db.JeepRoutes.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
                await db.MapObjects.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
                await db.SavedGames.All.Where(o => o.Id == savedGame.Id).ExecuteDeleteAsync();
                await db.Transactions.All.Where(o => o.SavedGameId == savedGame.Id).ExecuteDeleteAsync();
            }

            await transaction.CommitAsync();
            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<IEnumerable<SavedGame>> GetSavedGames()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return db.SavedGames.All;
    }

    public async Task<Result<SavedGame, ServiceError>> GetSavedGame(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var game = await db.SavedGames.All.FirstOrDefaultAsync(x => x.Id == id);
        if (game is null)
        {
            return new NotFoundError("Game not found");
        }

        return game;
    }

    public async Task<Result<SavedGame, ServiceError>> GetSavedGameByName(string name)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var game = await db.SavedGames.All.FirstOrDefaultAsync(x => x.Name == name);
        if (game is null)
        {
            return new NotFoundError("Game not found");
        }

        return game;
    }

    public async Task<Result<SavedGame, ServiceError>> CreateNewGame(string name, Difficulty difficulty, long width, long height)
    {
        var game = new SavedGame
        {
            Name = name,
            Difficulty = difficulty,
            MapWidth = width,
            MapHeight = height,
            MapSeed = Utils.GenerateEncryptionKey(),
            TimeOfDay = StartTime,
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
        
        var transactionResult = await transactionService.CreateTransaction(db, game, initialTransaction);
        return transactionResult.Map<Result<SavedGame, ServiceError>>(
            error => new ConflictError(error.Message),
            () => game);
    }

    public async Task<Option<ServiceError>> UpdateGame(SavedGame game)
    {
        if (game.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }
        
        await Task.CompletedTask;
        throw new NotImplementedException();
    }

    public async Task<Result<Animal, ServiceError>> PurchaseAnimal(SavedGame game, AnimalType type)
    {
        if (game.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var animal = new Animal()
            {
                Name = $"{type.Name}#{Utils.GeneratePassword(1, 4, Utils.Numbers)}",
                AnimalTypeId = type.Id,
                Sex = AnimalSex.Male,
                Age = 5,
                HasChip = false,
                Group = null,
                Health = 100,
                HungerLevel = 0,
                ThirstLevel = 0,
                PositionId = null,
                SavedGameId = game.Id
            };

            var animalResult = await animalService.CreateAnimal(db, game, animal);
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
            
            var transactionResult = await transactionService.CreateTransaction(db, game, purchase);
            if (transactionResult is Option<ServiceError>.Some transactionError)
            {
                await transaction.RollbackAsync();
                return transactionError.Value;
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

    public async Task<Result<Jeep, ServiceError>> PurchaseJeep(SavedGame game, decimal price)
    {
        if (game.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var jeep = new Jeep
            {
                SavedGameId = game.Id,
                SeatedVisitors = 0
            };

            var jeepResult = await jeepService.CreateJeep(db, game, jeep);
            if (jeepResult is Option<ServiceError>.Some jeepError)
            {
                await transaction.RollbackAsync();
                return new ConflictError(jeepError.Value.Message);
            }

            var purchase = new Transaction()
            {
                Type = TransactionType.Purchase,
                Amount = price,
                SavedGameId = game.Id
            };
            
            var transactionResult = await transactionService.CreateTransaction(db, game, purchase);
            if (transactionResult is Option<ServiceError>.Some transactionError)
            {
                await transaction.RollbackAsync();
                return transactionError.Value;
            }
            
            await transaction.CommitAsync();
            return Result<Jeep, ServiceError>.Success(jeep);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new BadRequestError(ex.Message);
        }
    }

    public async Task<Result<MapObject, ServiceError>> PurchaseMapObject(SavedGame game, MapObjectType type)
    {
        if (game.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        await using var db = await dbContextFactory.CreateDbContextAsync();
        await using var transaction = await db.Database.BeginTransactionAsync();

        try
        {
            var obj = new MapObject
            {
                PositionId = null,
                SavedGameId = game.Id,
                MapObjectTypeId = type.Id,
                ResourceAmount = type.ResourceAmount
            };

            var objectResult = await mapObjectService.CreateMapObject(db, game, obj);
            if (objectResult is Option<ServiceError>.Some objError)
            {
                await transaction.RollbackAsync();
                return new ConflictError(objError.Value.Message);
            }

            var purchase = new Transaction()
            {
                Type = TransactionType.Purchase,
                Amount = type.Price,
                SavedGameId = game.Id
            };
            
            var transactionResult = await transactionService.CreateTransaction(db, game, purchase);
            if (transactionResult is Option<ServiceError>.Some transactionError)
            {
                await transaction.RollbackAsync();
                return transactionError.Value;
            }
            
            await transaction.CommitAsync();
            return Result<MapObject, ServiceError>.Success(obj);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new BadRequestError(ex.Message);
        }
    }
}