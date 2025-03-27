using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IGameService
{
    //Test and whatnot
    Task<Option<ServiceError>> InitialiseComponents(
        List<AnimalType> animalTypes,
        List<MapObjectType> mapObjectTypes);
    Task<bool> PruneDatabase(SavedGame? savedGame);
    
    //Navigation
    Task LoadReference<TEntity, TProperty>(TEntity entity, 
        Expression<Func<TEntity, TProperty?>> selection)
        where TEntity : class
        where TProperty : class;
    Task LoadCollection<TEntity, TProperty>(TEntity entity, 
        Expression<Func<TEntity, IEnumerable<TProperty>>> selection)
        where TEntity : class
        where TProperty : class;
    
    //Games
    Task<List<SavedGame>> GetSavedGames();
    Task<Result<SavedGame, ServiceError>> GetSavedGame(long id);
    Task<Result<SavedGame, ServiceError>> GetSavedGameByName(string name);
    Task<Result<SavedGame, ServiceError>> CreateNewGame(string name, Difficulty difficulty, 
        long width, long height);
    Task<Option<ServiceError>> UpdateGame(SavedGame game);
    Task<Option<ServiceError>> DeleteGame(SavedGame game);
    
    //Animal types
    Task<List<AnimalType>> GetAnimalTypes();
    Task<Result<AnimalType, ServiceError>> GetAnimalTypeById(long id);
    Task<Option<ServiceError>> CreateAnimalType(AnimalType animalType);
    Task<Option<ServiceError>> UpdateAnimalType(AnimalType animalType);
    Task<Option<ServiceError>> DeleteAnimalType(AnimalType animalType);
    
    //Animals
    Task<List<Animal>> GetAnimals(SavedGame game);
    Task<Result<Animal, ServiceError>> GetAnimalById(long id);
    Task<Result<Animal, ServiceError>> PurchaseAnimal(SavedGame game, AnimalType type, Position position);
    Task<Option<ServiceError>> SellAnimal(SavedGame game, Animal animal, decimal refundPrice);
    Task<Option<ServiceError>> UpdateAnimal(Animal animal);
    Task<Option<ServiceError>> DeleteAnimal(Animal animal);
    
    //Animal groups
    Task<List<AnimalGroup>> GetGroups(SavedGame savedGame);
    Task<Option<ServiceError>> CreateGroup(SavedGame savedGame, AnimalGroup group);
    Task<Option<ServiceError>> UpdateGroup(AnimalGroup group);
    Task<Option<ServiceError>> DeleteGroup(AnimalGroup group);
    Task<Option<ServiceError>> AddAnimalToGroup(Animal animal, AnimalGroup group);
    Task<Option<ServiceError>> RemoveAnimalFromGroup(Animal animal, AnimalGroup group);
    
    //Discoveries
    Task<List<MapObject>> GetDiscoveredMapObjects(Animal animal);
    Task<Option<ServiceError>> DiscoverMapObject(Animal animal, MapObject mapObject);
    
    //Jeeps
    Task<List<Jeep>> GetJeeps(SavedGame game);
    Task<Result<Jeep, ServiceError>> GetJeepById(long id);
    Task<Result<Jeep, ServiceError>> PurchaseJeep(SavedGame game, [Range(0, Double.MaxValue)] decimal price);
    Task<Option<ServiceError>> SellJeep(SavedGame game, Jeep jeep, decimal refundPrice);
    Task<Option<ServiceError>> UpdateJeep(Jeep jeep);
    Task<Option<ServiceError>> DeleteJeep(Jeep jeep);
    
    //Jeep routes
    Task<List<JeepRoute>> GetRoutes(SavedGame game);
    Task<Option<ServiceError>> CreateRoute(SavedGame savedGame, JeepRoute route);
    Task<Option<ServiceError>> UpdateRoute(JeepRoute route);
    Task<Option<ServiceError>> DeleteRoute(JeepRoute route);
    
    //Jeep-Route Interaction
    Task<Option<ServiceError>> StartRoute(JeepRoute route, Jeep jeep);
    Task<Option<ServiceError>> FinishRoute(Jeep jeep);
    
    //Map object types
    Task<List<MapObjectType>> GetMapObjectTypes();
    Task<Result<MapObjectType, ServiceError>> GetMapObjectTypeById(long id);
    Task<Option<ServiceError>> CreateMapObjectType(MapObjectType mapObjectType);
    Task<Option<ServiceError>> UpdateMapObjectType(MapObjectType mapObjectType);
    Task<Option<ServiceError>> DeleteMapObjectType(MapObjectType mapObjectType);
    
    //MapObjects
    Task<List<MapObject>> GetMapObjects(SavedGame game);
    Task<Result<MapObject, ServiceError>> GetMapObjectById(long id);
    Task<Result<MapObject, ServiceError>> PurchaseMapObject(SavedGame game, MapObjectType type, Position position);
    Task<Option<ServiceError>> SellMapObject(SavedGame game, MapObject mapObject, decimal refundPrice);
    Task<Option<ServiceError>> UpdateMapObject(MapObject mapObject);
    Task<Option<ServiceError>> DeleteMapObject(MapObject mapObject);
    
    //Positions
    Task<List<Position>> GetPositions();
    Task<Result<Position, ServiceError>> GetPositionById(long id);
    Task<Option<ServiceError>> CreatePosition(Position position);
    Task<Option<ServiceError>> UpdatePosition(Position position);
    Task<Option<ServiceError>> DeletePosition(Position position);
    
    //Transactions
    Task<List<Transaction>> GetTransactionsFromLastCheckpoint(SavedGame savedGame);
    Task<List<Transaction>> GetAllTransactions(SavedGame savedGame);
    Task<Result<Transaction, ServiceError>> GetTransactionById(long id);
    Task<Result<decimal, ServiceError>> GetCurrentBalance(SavedGame savedGame);
    Task<Result<Transaction, ServiceError>> CreateCheckpoint(SavedGame savedGame);
    Task<Option<ServiceError>> CreateTransaction(SavedGame savedGame, 
        Transaction transaction, bool allowLose = false);
}

//Minden tipushoz
//GetAll
//GetById
//Create
//Update
//Delete

//Modelben tortenik valami
// frissitjuk a jatekot
// create, delete -> rogton mentunk
// mentesi ciklusok (idokozonkent)
// explicit mentes
// window actionoknel mentes