using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Model.Validators;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Model;

public class JurassicParkModel(IGameService gameService, IRouteValidator routeValidator) : IJurassicParkModel
{
    public IGameService GameService => gameService;
    
    public decimal JeepPrice => 1000;
    public decimal RouteBlockPrice => 50;

    public List<Animal> Animals { get; private set; } = null!;
    public List<AnimalGroup> AnimalGroups { get; private set; } = null!;
    public List<AnimalType> AnimalTypes { get; private set; } = null!;
    public List<Jeep> Jeeps { get; private set; } = null!;
    public List<JeepRoute> JeepRoutes { get; private set; } = null!;
    public List<MapObject> MapObjects { get; private set; } = null!;
    public List<MapObjectType> MapObjectTypes { get; private set; } = null!;
    public List<Position> Positions { get; private set; } = null!;
    public List<Transaction> Transactions { get; private set; } = null!;
    
    public SavedGame? SavedGame { get; private set; }
    
    public async Task<IEnumerable<SavedGame>> LoadSavedGamesAsync()
    {
        return await gameService.GetSavedGames();
    }

    public async Task<Option<ServiceError>> LoadGameAsync(long id)
    {
        if (SavedGame is not null)
            return new ConflictError("Game already loaded");
        
        var game = await gameService.GetSavedGame(id);
        if (game.IsError)
        {
            return game.GetErrorOrThrow();
        }
        
        SavedGame = game.GetValueOrThrow();
        
        Animals = await gameService.GetAnimals(SavedGame);
        AnimalGroups = await gameService.GetGroups(SavedGame);
        AnimalTypes = await gameService.GetAnimalTypes();
        Jeeps = await gameService.GetJeeps(SavedGame);
        JeepRoutes = await gameService.GetRoutes(SavedGame);
        MapObjects = await gameService.GetMapObjects(SavedGame);
        MapObjectTypes = await gameService.GetMapObjectTypes();
        Positions = await gameService.GetPositions();
        Transactions = await gameService.GetAllTransactions(SavedGame);
        
        return new Option<ServiceError>.None();
    }

    public async Task<Result<SavedGame, ServiceError>> CreateGameAsync(string name, 
        Difficulty difficulty, long mapWidth, long mapHeight)
    {
        if (SavedGame is not null)
            return new ConflictError("Game already exists");
        
        var game = await gameService.CreateNewGame(name, difficulty, mapWidth, mapHeight);
        if (game.HasValue)
        {
            SavedGame = game.GetValueOrThrow();
        }

        return game;
    }

    public async Task<Option<ServiceError>> DeleteGameAsync()
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        Animals.Clear();
        AnimalGroups.Clear();
        Jeeps.Clear();
        JeepRoutes.Clear();
        MapObjects.Clear();
        Transactions.Clear();
        
        var result = await gameService.DeleteGame(SavedGame);
        SavedGame = null;
        return result;
    }

    public async Task SaveAsync()
    {
        if (SavedGame?.GameState != GameState.Ongoing)
            return;
        
        foreach (var animal in Animals)
        {
            await gameService.UpdateAnimal(animal);
        }
        
        foreach (var group in AnimalGroups)
        {
            await gameService.UpdateGroup(group);
        }
        
        foreach (var jeep in Jeeps)
        {
            await gameService.UpdateJeep(jeep);
        }
        
        foreach (var jeepRoute in JeepRoutes)
        {
            await gameService.UpdateRoute(jeepRoute);
        }
        
        foreach (var mapObject in MapObjects)
        {
            await gameService.UpdateMapObject(mapObject);
        }
        
        foreach (var mapObjectType in MapObjectTypes)
        {
            await gameService.UpdateMapObjectType(mapObjectType);
        }

        foreach (var position in Positions)
        {
            await gameService.UpdatePosition(position);
        }
        
        await gameService.UpdateGame(SavedGame);
    }

    public async Task<Result<Animal, ServiceError>> PurchaseAnimal(AnimalType animalType)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        var animal = await gameService.PurchaseAnimal(SavedGame, animalType);
        if (animal.HasValue)
        {
            Animals.Add(animal.GetValueOrThrow());
        }
        
        return animal;
    }

    public async Task<Option<ServiceError>> SellAnimal(Animal animal)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");

        await gameService.LoadReference(animal, a => a.AnimalType);
        var original = animal.AnimalType.Price;
        var age = animal.Age;
        var refundPrice = original * (decimal)Math.Cbrt(1.0 / (age + 1));

        Animals.Remove(animal);
        return await gameService.SellAnimal(SavedGame, animal, refundPrice);
    }

    public async Task<Option<ServiceError>> KillAnimal(Animal animal)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");

        Animals.Remove(animal);
        return await gameService.DeleteAnimal(animal);
    }

    public async Task<Result<MapObject, ServiceError>> PurchaseMapObject(MapObjectType mapObjectType)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        var obj = await gameService.PurchaseMapObject(SavedGame, mapObjectType);
        if (obj.HasValue)
        {
            MapObjects.Add(obj.GetValueOrThrow());
        }
        
        return obj;
    }

    public async Task<Option<ServiceError>> SellMapObject(MapObject mapObject)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");

        //Calculate refund by lerping
        await gameService.LoadReference(mapObject, mo => mo.MapObjectType);
        var original = mapObject.MapObjectType.Price;
        var resourcePercent = mapObject.ResourceAmount / mapObject.MapObjectType.ResourceAmount;
        var refundPrice = original * resourcePercent;
        
        MapObjects.Remove(mapObject);
        return await gameService.SellMapObject(SavedGame, mapObject, refundPrice);
    }

    public async Task<Option<ServiceError>> RemoveMapObject(MapObject mapObject)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");

        MapObjects.Remove(mapObject);
        return await gameService.DeleteMapObject(mapObject);
    }

    public async Task<Result<Jeep, ServiceError>> PurchaseJeep()
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        var obj = await gameService.PurchaseJeep(SavedGame, JeepPrice);
        if (obj.HasValue)
        {
            Jeeps.Add(obj.GetValueOrThrow());
        }
        
        return obj;
    }

    public async Task<Option<ServiceError>> SellJeep(Jeep jeep)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        Jeeps.Remove(jeep);
        return await gameService.SellJeep(SavedGame, jeep, JeepPrice);
    }

    public async Task<Option<ServiceError>> DuplicateRoute(JeepRoute jeepRoute, Position position)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        var contains = await routeValidator.IsPositionOnRoute(jeepRoute, position);
        if (!contains) return new BadRequestError("Route doesn't have this position");
        
        //TODO
        throw new NotImplementedException();
    }

    public async Task<Option<ServiceError>> PurchaseRoadBlock(JeepRoute jeepRoute, Position position)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        var canPurchase = await routeValidator.CanAddRouteBlock(jeepRoute, position);
        if (!canPurchase)
        {
            return new BadRequestError("Can not remove any more elements");
        } 
        
        //TODO
        return new Option<ServiceError>.None();
    }

    public async Task<Option<ServiceError>> SellRoadBlock(JeepRoute jeepRoute)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");

        var canRemove = await routeValidator.CanRemoveElement(jeepRoute);
        if (canRemove)
        {
            //TODO remove
            //TODO is empty, delete
        }
        
        return  new BadRequestError("Can not remove any more elements");
    }

    public async Task UpdateAsync(double delta)
    {
        if (SavedGame?.GameState != GameState.Ongoing)
            return;
        
        throw new NotImplementedException();
    }  
}