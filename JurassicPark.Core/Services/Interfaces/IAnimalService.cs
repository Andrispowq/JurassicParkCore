using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IAnimalService
{
    //Animal types
    public IEnumerable<AnimalType> GetAllAnimalTypes(JurassicParkDbContext context);
    public Task<Option<ServiceError>> CreateAnimalType(JurassicParkDbContext context, AnimalType animalType);
    //Animals
    public IEnumerable<Animal> GetAnimals(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Result<Animal, ServiceError>> GetAnimalById(JurassicParkDbContext context, long animalId);
    public Task<Option<ServiceError>> CreateAnimal(JurassicParkDbContext context, SavedGame savedGame, Animal animal);
    public Task<Option<ServiceError>> UpdateAnimal(JurassicParkDbContext context, Animal animal);
    //Groups
    public IEnumerable<AnimalGroup> GetGroups(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Option<ServiceError>> CreateGroup(JurassicParkDbContext context, SavedGame savedGame, AnimalGroup group);
    public Task<Option<ServiceError>> DeleteGroup(JurassicParkDbContext context, AnimalGroup group);
    public Task<Option<ServiceError>> AddAnimalToGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
    public Task<Option<ServiceError>> RemoveAnimalFromGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
    //Discoveries
    public Task<IEnumerable<MapObject>> GetDiscoveredMapObjects(JurassicParkDbContext context, Animal animal);
    public Task<Option<ServiceError>> DiscoverMapObject(JurassicParkDbContext context, Animal animal, MapObject mapObject);
}