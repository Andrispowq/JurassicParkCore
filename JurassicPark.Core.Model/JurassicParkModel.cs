using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Model;

public class JurassicParkModel(IGameService gameService) : IJurassicParkModel
{
    public decimal JeepPrice => 1000;
    
    public event EventHandler<SavedGame>? GameCreated; 
    public event EventHandler<string>? GameWarning;
    public event EventHandler? GameGoalMet;
    public event EventHandler<bool>? GameOver;

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

    public async Task<Option<ServiceError>> LoadGamesAsync(string name)
    {
        if (SavedGame is not null)
            return new ConflictError("Game already loaded");
        
        var game = await gameService.GetSavedGameByName(name);
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

    public async Task<Option<ServiceError>> PurchaseAnimal(AnimalType animalType)
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
        
        return animal.IsError ? animal.GetErrorOrThrow() : new Option<ServiceError>.None();
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
        
        return await gameService.SellAnimal(SavedGame, animal, refundPrice);
    }

    public async Task<Option<ServiceError>> PurchaseMapObject(MapObjectType mapObjectType)
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
        
        return obj.IsError ? obj.GetErrorOrThrow() : new Option<ServiceError>.None();
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
        
        return await gameService.SellMapObject(SavedGame, mapObject, refundPrice);
    }

    public async Task<Option<ServiceError>> PurchaseJeep()
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
        
        return obj.IsError ? obj.GetErrorOrThrow() : new Option<ServiceError>.None();
    }

    public async Task<Option<ServiceError>> SellJeep(Jeep jeep)
    {
        if (SavedGame is null)
            return new NotFoundError("No game active");
        if (SavedGame.GameState != GameState.Ongoing)
            return new UnauthorizedError("Game is already over");
        
        return await gameService.SellJeep(SavedGame, jeep, JeepPrice);
    }
    
    public async Task UpdateAsync(double delta)
    {
        if (SavedGame?.GameState != GameState.Ongoing)
            return;
        
        await Task.CompletedTask;
    }  
}