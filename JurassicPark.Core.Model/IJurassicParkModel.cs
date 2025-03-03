using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Model;

public interface IJurassicParkModel
{
    decimal JeepPrice { get; }
    
    //Game events
    event EventHandler<SavedGame>? GameCreated; //The Game created 
    event EventHandler<string>? GameWarning; //The warning message
    event EventHandler? GameGoalMet; 
    event EventHandler<bool>? GameOver; //True is win, false is lose

    //Game objects
    List<Animal> Animals { get; }
    List<AnimalGroup> AnimalGroups { get; }
    List<AnimalType> AnimalTypes { get; }
    List<Jeep> Jeeps { get; }
    List<JeepRoute> JeepRoutes { get; }
    List<MapObject> MapObjects { get; }
    List<MapObjectType> MapObjectTypes { get; }
    List<Position> Positions { get; }
    List<Transaction> Transactions { get; }
    SavedGame? SavedGame { get; }

    //Game stuff
    Task<IEnumerable<SavedGame>> LoadSavedGamesAsync();
    Task<Option<ServiceError>> LoadGamesAsync(string name);
    Task<Result<SavedGame, ServiceError>> CreateGameAsync(string name, Difficulty difficulty, long mapWidth, long mapHeight);
    Task<Option<ServiceError>> DeleteGameAsync();

    //Update and save
    Task UpdateAsync(double delta);
    Task SaveAsync();
    
    //Transactional stuff
    Task<Option<ServiceError>> PurchaseAnimal(AnimalType animalType);
    Task<Option<ServiceError>> SellAnimal(Animal animal);
    
    Task<Option<ServiceError>> PurchaseMapObject(MapObjectType mapObjectType);
    Task<Option<ServiceError>> SellMapObject(MapObject mapObject);
    
    Task<Option<ServiceError>> PurchaseJeep();
    Task<Option<ServiceError>> SellJeep(Jeep jeep);
    
    //TODO
    //Task<Option<ServiceError>> PurchaseJeepRoute(JeepRoute jeepRoute, Position position);
    //Sell?
}