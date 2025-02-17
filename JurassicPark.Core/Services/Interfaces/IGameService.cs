using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services.Interfaces;

public interface IGameService
{
    IAnimalService AnimalService { get; }
    IJeepService JeepService { get; }
    IMapObjectService MapObjectService { get; }
    IPositionService PositionService { get; }
    ITransactionService TransactionService { get; }
    
    Task<JurassicParkDbContext> CreateDbContextAsync();
    
    //Setup, only call once
    Task<Option<ServiceError>> InitialiseComponents(
        List<AnimalType> animalTypes,
        List<MapObjectType> mapObjectTypes);

    Task<bool> PruneDatabase(SavedGame? savedGame);
    
    //Games
    Task<IEnumerable<SavedGame>> GetSavedGames();
    Task<Result<SavedGame, ServiceError>> GetSavedGame(long id);
    Task<Result<SavedGame, ServiceError>> GetSavedGameByName(string name);
    
    //Create new save
    Task<Result<SavedGame, ServiceError>> CreateNewGame(string name, Difficulty difficulty, 
        long width, long height);
    Task<Option<ServiceError>> UpdateGame(SavedGame game);
    
    //Purchases
    Task<Result<Animal, ServiceError>> PurchaseAnimal(SavedGame game, AnimalType type);
    Task<Result<Jeep, ServiceError>> PurchaseJeep(SavedGame game);
    Task<Result<MapObject, ServiceError>> PurchaseMapObject(SavedGame game, MapObjectType type);
    
    //Jeep interactions
}