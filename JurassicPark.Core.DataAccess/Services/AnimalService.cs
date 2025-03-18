using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Core.Services;

public class AnimalService : IAnimalService
{
    public IEnumerable<AnimalType> GetAllAnimalTypes(JurassicParkDbContext context)
    {
        return context.AnimalTypes.All;
    }

    public async Task<Result<AnimalType, ServiceError>> GetAnimalTypeById(JurassicParkDbContext context, long animalTypeId)
    {
        var animal = await context.AnimalTypes.Get(animalTypeId);
        return animal.Map<Result<AnimalType, ServiceError>>(value => value, error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> CreateAnimalType(JurassicParkDbContext context, AnimalType animalType)
    {
        var result = await context.AnimalTypes.Create(animalType);
        return result.MapOption<ServiceError>(
            some => new ConflictError(some.Message));
    }

    public async Task<Option<ServiceError>> UpdateAnimalType(JurassicParkDbContext context, AnimalType animalType)
    {
        var result = await context.AnimalTypes.Update(animalType);
        return result.MapOption<ServiceError>(
            some => new NotFoundError(some.Message));
    }

    public async Task<Option<ServiceError>> DeleteAnimalType(JurassicParkDbContext context, AnimalType animalType)
    {
        var result = await context.AnimalTypes.Delete(animalType);
        return result.MapOption<ServiceError>(
            some => new NotFoundError(some.Message));
    }

    public IEnumerable<Animal> GetAnimals(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.Animals.All
            .Include(a => a.AnimalType)
            .Include(a => a.Position)
            .Include(a => a.PointOfInterest)
            .Include(a => a.Group)
            .Where(a => a.SavedGame.Id == savedGame.Id);
    }

    public async Task<Result<Animal, ServiceError>> GetAnimalById(JurassicParkDbContext context, long animalId)
    {
        var animal = await context.Animals.Get(animalId);
        return animal.Map<Result<Animal, ServiceError>>(value => value, 
            error => new NotFoundError(error.Message));
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

    public async Task<Option<ServiceError>> DeleteAnimal(JurassicParkDbContext context, Animal animal)
    {
        var result = await context.Animals.Delete(animal);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
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

    public async Task<Option<ServiceError>> UpdateGroup(JurassicParkDbContext context, AnimalGroup group)
    {
        await context.Entry(group).Reference(a => a.SavedGame).LoadAsync();
        if (group.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.AnimalGroups.Update(group);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
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

    public async Task<IEnumerable<MapObject>> GetDiscoveredMapObjects(JurassicParkDbContext context, Animal animal)
    {
        List<MapObject> mapObjects = new();

        var discovered = context.Discoveries.All
            .Where(d => d.AnimalId == animal.Id);
        
        foreach (var discovery in discovered)
        {
            await context.Entry(discovery).Reference(a => a.MapObject).LoadAsync();
            mapObjects.Add(discovery.MapObject);
        }
        
        return mapObjects;
    }

    public async Task<Option<ServiceError>> DiscoverMapObject(JurassicParkDbContext context, Animal animal, MapObject mapObject)
    {
        await context.Entry(animal).Reference(a => a.SavedGame).LoadAsync();
        if (animal.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        if (animal.SavedGameId != mapObject.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        
        await context.Entry(animal).Collection(a => a.DiscoveredMapObjects).LoadAsync();
        if (animal.DiscoveredMapObjects.Any(d => d.MapObjectId == mapObject.Id))
        {
            return new ConflictError("This object is already discovered");
        }

        var discovery = new Discovered
        {
            AnimalId = animal.Id,
            MapObjectId = mapObject.Id
        };
        
        var error = await context.Discoveries.Create(discovery);
        if (error.IsSome) return new ConflictError(error.AsSome.Value.Message);
            
        return new Option<ServiceError>.None();
    }
}