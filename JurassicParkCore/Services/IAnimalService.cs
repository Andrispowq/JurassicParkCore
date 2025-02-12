using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services;

public interface IAnimalService
{
    public IEnumerable<AnimalType> GetAllAnimalTypes(JurassicParkDbContext context);
    public Task<Option<ServiceError>> CreateAnimalType(JurassicParkDbContext context, AnimalType animalType);
    public IEnumerable<Animal> GetAnimals(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Option<ServiceError>> CreateAnimal(JurassicParkDbContext context, Animal animal);
    public Task<Option<ServiceError>> UpdateAnimal(JurassicParkDbContext context, Animal animal);
    public IEnumerable<AnimalGroup> GetGroups(JurassicParkDbContext context, SavedGame savedGame);
    public Task<Option<ServiceError>> CreateGroup(JurassicParkDbContext context, AnimalGroup group);
    public Task<Option<ServiceError>> DeleteGroup(JurassicParkDbContext context, AnimalGroup group);
    public Task<Option<ServiceError>> AddAnimalToGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
    public Task<Option<ServiceError>> RemoveAnimalFromGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group);
}