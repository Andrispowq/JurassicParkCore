using System.Linq.Expressions;
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
    ITransactionService transactionService,
    IRandomValueProvider randomValueProvider) : IGameService
{
    public const decimal InitialMoney = 100000;
    public static readonly TimeOnly StartTime = new(9, 0, 0);

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
                await db.AnimalTypes.All.ExecuteDeleteAsync();
                await db.MapObjectTypes.All.ExecuteDeleteAsync();
                await db.Positions.All.ExecuteDeleteAsync();
                await db.SavedGames.All.ExecuteDeleteAsync();
            }
            else
            {
                await db.SavedGames.All.Where(o => o.Id == savedGame.Id).ExecuteDeleteAsync();
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

    public async Task LoadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> selection) 
        where TEntity : class
        where TProperty : class
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.Entry(entity).Reference(selection).LoadAsync();
    }

    public async Task LoadCollection<TEntity, TProperty>(TEntity entity,
        Expression<Func<TEntity, IEnumerable<TProperty>>> selection)
        where TEntity : class
        where TProperty : class
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        await db.Entry(entity).Collection(selection).LoadAsync();
    }

    public async Task<List<SavedGame>> GetSavedGames()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await db.SavedGames.All.ToListAsync();
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
        
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.SavedGames.Update(game);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteGame(SavedGame game)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        var result = await db.SavedGames.Delete(game);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<List<AnimalType>> GetAnimalTypes()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return animalService.GetAllAnimalTypes(db).ToList();
    }

    public async Task<Result<AnimalType, ServiceError>> GetAnimalTypeById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.GetAnimalTypeById(db, id);
    }

    public async Task<Option<ServiceError>> CreateAnimalType(AnimalType animalType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.CreateAnimalType(db, animalType);
    }

    public async Task<Option<ServiceError>> UpdateAnimalType(AnimalType animalType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.UpdateAnimalType(db, animalType);
    }

    public async Task<Option<ServiceError>> DeleteAnimalType(AnimalType animalType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.DeleteAnimalType(db, animalType);
    }

    public async Task<List<Animal>> GetAnimals(SavedGame game)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return animalService.GetAnimals(db, game).ToList();
    }

    public async Task<Result<Animal, ServiceError>> GetAnimalById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.GetAnimalById(db, id);
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
                Sex = randomValueProvider.GetSexFor(type),
                Age = 0,
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

    public async Task<Option<ServiceError>> UpdateAnimal(Animal animal)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.UpdateAnimal(db, animal);
    }

    public async Task<Option<ServiceError>> DeleteAnimal(Animal animal)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.DeleteAnimal(db, animal);
    }

    public async Task<List<AnimalGroup>> GetGroups(SavedGame savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return animalService.GetGroups(db, savedGame).ToList();
    }

    public async Task<Option<ServiceError>> CreateGroup(SavedGame savedGame, AnimalGroup group)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.CreateGroup(db, savedGame, group);
    }

    public async Task<Option<ServiceError>> DeleteGroup(AnimalGroup group)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.DeleteGroup(db, group);
    }

    public async Task<Option<ServiceError>> AddAnimalToGroup(Animal animal, AnimalGroup group)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.AddAnimalToGroup(db, animal, group);
    }

    public async Task<Option<ServiceError>> RemoveAnimalFromGroup(Animal animal, AnimalGroup group)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.RemoveAnimalFromGroup(db, animal, group);
    }

    public async Task<List<MapObject>> GetDiscoveredMapObjects(Animal animal)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return (await animalService.GetDiscoveredMapObjects(db, animal)).ToList();
    }

    public async Task<Option<ServiceError>> DiscoverMapObject(Animal animal, MapObject mapObject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await animalService.DiscoverMapObject(db, animal, mapObject);
    }

    public async Task<List<Jeep>> GetJeeps(SavedGame game)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return jeepService.GetJeeps(db, game).ToList();
    }

    public async Task<Result<Jeep, ServiceError>> GetJeepById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.GetJeepById(db, id);
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

    public async Task<Option<ServiceError>> UpdateJeep(Jeep jeep)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.UpdateJeep(db, jeep);
    }

    public async Task<Option<ServiceError>> DeleteJeep(Jeep jeep)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.DeleteJeep(db, jeep);
    }

    public async Task<List<JeepRoute>> GetRoutes(SavedGame game)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return jeepService.GetRoutes(db, game).ToList();
    }

    public async Task<Option<ServiceError>> CreateRoute(SavedGame savedGame, JeepRoute route)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.CreateRoute(db, savedGame, route);
    }

    public async Task<Option<ServiceError>> UpdateRoute(JeepRoute route)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.UpdateRoute(db, route);
    }

    public async Task<Option<ServiceError>> DeleteRoute(JeepRoute route)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.DeleteRoute(db, route);
    }

    public async Task<Option<ServiceError>> StartRoute(JeepRoute route, Jeep jeep)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.StartRoute(db, route, jeep);
    }

    public async Task<Option<ServiceError>> FinishRoute(Jeep jeep)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await jeepService.FinishRoute(db, jeep);
    }

    public async Task<List<MapObjectType>> GetMapObjectTypes()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return mapObjectService.GetMapObjectTypes(db).ToList();
    }

    public async Task<Result<MapObjectType, ServiceError>> GetMapObjectTypeById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.GetMapObjectTypeById(db, id);
    }

    public async Task<Option<ServiceError>> CreateMapObjectType(MapObjectType mapObjectType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.CreateMapObjectType(db, mapObjectType);
    }

    public async Task<Option<ServiceError>> UpdateMapObjectType(MapObjectType mapObjectType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.UpdateMapObjectType(db, mapObjectType);
    }

    public async Task<Option<ServiceError>> DeleteMapObjectType(MapObjectType mapObjectType)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.DeleteMapObjectType(db, mapObjectType);
    }

    public async Task<List<MapObject>> GetMapObjects(SavedGame game)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return mapObjectService.GetMapObjects(db, game).ToList();
    }

    public async Task<Result<MapObject, ServiceError>> GetMapObjectById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.GetMapObjectById(db, id);
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

    public async Task<Option<ServiceError>> UpdateMapObject(MapObject mapObject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.UpdateMapObject(db, mapObject);
    }

    public async Task<Option<ServiceError>> DeleteMapObject(MapObject mapObject)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await mapObjectService.DeleteMapObject(db, mapObject);
    }

    public async Task<List<Position>> GetPositions()
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return positionService.GetPositions(db).ToList();
    }

    public async Task<Result<Position, ServiceError>> GetPositionById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await positionService.GetPositionById(db, id);
    }

    public async Task<Option<ServiceError>> CreatePosition(Position position)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await positionService.CreatePosition(db, position);
    }

    public async Task<Option<ServiceError>> UpdatePosition(Position position)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await positionService.UpdatePosition(db, position);
    }

    public async Task<Option<ServiceError>> DeletePosition(Position position)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await positionService.DeletePosition(db, position);
    }

    public async Task<List<Transaction>> GetTransactionsFromLastCheckpoint(SavedGame savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return transactionService.GetTransactionsFromLastCheckpoint(db, savedGame).ToList();
    }

    public async Task<List<Transaction>> GetAllTransactions(SavedGame savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return transactionService.GetAllTransactions(db, savedGame).ToList();
    }

    public async Task<Result<Transaction, ServiceError>> GetTransactionById(long id)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await transactionService.GetTransactionById(db, id);
    }

    public async Task<Result<decimal, ServiceError>> GetCurrentBalance(SavedGame savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return transactionService.GetCurrentBalance(db, savedGame);
    }

    public async Task<Result<Transaction, ServiceError>> CreateCheckpoint(SavedGame savedGame)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await transactionService.CreateCheckpoint(db, savedGame);
    }

    public async Task<Option<ServiceError>> CreateTransaction(SavedGame savedGame, Transaction transaction,
        bool allowLose = false)
    {
        await using var db = await dbContextFactory.CreateDbContextAsync();
        return await transactionService.CreateTransaction(db, savedGame, transaction, allowLose);
    }
}