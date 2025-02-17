using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Services;

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

    public async Task<Option<ServiceError>> CreateAnimal(JurassicParkDbContext context, SavedGame savedGame, Animal animal)
    {
        if (savedGame.Id != animal.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.Animals.Create(animal);
        return result.MapOption<ServiceError>(
            error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateAnimal(JurassicParkDbContext context, Animal animal)
    {
        await context.Entry(animal).Reference(a => a.SavedGame).LoadAsync();
        if (animal.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

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

    public async Task<Option<ServiceError>> CreateGroup(JurassicParkDbContext context, SavedGame savedGame, AnimalGroup group)
    {
        if (savedGame.Id != group.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        await context.Entry(group).Reference(a => a.SavedGame).LoadAsync();
        if (group.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.AnimalGroups.Create(group);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteGroup(JurassicParkDbContext context, AnimalGroup group)
    {
        await context.Entry(group).Reference(a => a.SavedGame).LoadAsync();
        if (group.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.AnimalGroups.Delete(group);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> AddAnimalToGroup(JurassicParkDbContext context, Animal animal, AnimalGroup group)
    {
        await context.Entry(group).Reference(a => a.SavedGame).LoadAsync();
        if (group.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

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
        await context.Entry(group).Reference(a => a.SavedGame).LoadAsync();
        if (group.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        if (animal.Group is null)
        {
            return new BadRequestError("This animal is not in a group");
        }
        
        animal.Group = null;
        
        var result = await context.Animals.Update(animal);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }
}