using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services;

public class AnimalService : IAnimalService
{
    public IEnumerable<AnimalType> GetAllAnimalTypes(JurassicParkDbContext context)
    {
        return context.AnimalTypes.All;
    }

    public async Task<Option<ServiceError>> CreateAnimalType(JurassicParkDbContext context, AnimalType animalType)
    {
        var result = await context.AnimalTypes.Create(animalType);
        return result.MapOption<ServiceError>(
            some => new ConflictError(some.Message));
    }

    public IEnumerable<Animal> GetAnimals(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.Animals.All.Where(a => a.SavedGame.Id == savedGame.Id);
    }

    public async Task<Option<ServiceError>> CreateAnimal(JurassicParkDbContext context, Animal animal)
    {
        var result = await context.Animals.Create(animal);
        return result.MapOption<ServiceError>(
            error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateAnimal(JurassicParkDbContext context, Animal animal)
    {
        if (animal.Health <= 0)
        {
            var result = await context.Animals.Delete(animal);
            return result.MapOption<ServiceError>(
                error => new NotFoundError(error.Message));
        }
        else
        {
            var result = await context.Animals.Update(animal);
            return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
        }
    }

    public IEnumerable<AnimalGroup> GetGroups(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.AnimalGroups.All.Where(a => a.SavedGame.Id == savedGame.Id);
    }

    public async Task<Option<ServiceError>> CreateGroup(JurassicParkDbContext context, AnimalGroup group)
    {
        var result = await context.AnimalGroups.Create(group);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteGroup(JurassicParkDbContext context, AnimalGroup group)
    {
        var result = await context.AnimalGroups.Delete(group);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> AddAnimalToGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group)
    {
        if (animal.Group is not null)
        {
            return new BadRequestError("This animal is already in a group");
        }
        
        animal.Group = group;
        
        var result = await context.Animals.Update(animal);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> RemoveAnimalFromGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group)
    {
        if (animal.Group is null)
        {
            return new BadRequestError("This animal is not in a group");
        }
        
        animal.Group = null;
        
        var result = await context.Animals.Update(animal);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }
}