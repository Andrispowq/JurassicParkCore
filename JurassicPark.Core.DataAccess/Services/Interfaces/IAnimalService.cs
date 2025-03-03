using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IAnimalService
{
    //Animal types
    IEnumerable<AnimalType> GetAllAnimalTypes(JurassicParkDbContext context);
    Task<Result<AnimalType, ServiceError>> GetAnimalTypeById(JurassicParkDbContext context, long animalTypeId);
    Task<Option<ServiceError>> CreateAnimalType(JurassicParkDbContext context, AnimalType animalType);
    Task<Option<ServiceError>> UpdateAnimalType(JurassicParkDbContext context, AnimalType animalType);
    Task<Option<ServiceError>> DeleteAnimalType(JurassicParkDbContext context, AnimalType animalType);
    
    //Animals
    IEnumerable<Animal> GetAnimals(JurassicParkDbContext context, SavedGame savedGame);
    Task<Result<Animal, ServiceError>> GetAnimalById(JurassicParkDbContext context, long animalId);
    Task<Option<ServiceError>> CreateAnimal(JurassicParkDbContext context, SavedGame savedGame, Animal animal);
    Task<Option<ServiceError>> UpdateAnimal(JurassicParkDbContext context, Animal animal);
    Task<Option<ServiceError>> DeleteAnimal(JurassicParkDbContext context, Animal animal);
    //Groups
    IEnumerable<AnimalGroup> GetGroups(JurassicParkDbContext context, SavedGame savedGame);
    Task<Option<ServiceError>> CreateGroup(JurassicParkDbContext context, SavedGame savedGame, AnimalGroup group);
    Task<Option<ServiceError>> UpdateGroup(JurassicParkDbContext context, AnimalGroup group);
    Task<Option<ServiceError>> DeleteGroup(JurassicParkDbContext context, AnimalGroup group);
    Task<Option<ServiceError>> AddAnimalToGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
    Task<Option<ServiceError>> RemoveAnimalFromGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
    //Discoveries
    Task<IEnumerable<MapObject>> GetDiscoveredMapObjects(JurassicParkDbContext context, Animal animal);
    Task<Option<ServiceError>> DiscoverMapObject(JurassicParkDbContext context, Animal animal, MapObject mapObject);
}