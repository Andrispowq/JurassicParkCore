using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Extensions;

namespace JurassicPark.Test.Unit;

[TestFixture]
public class AnimalTests : GameRequiredTest
{
    [Test]
    public async Task CreateAnimal()
    {
        await GameService.PruneDatabase(null);
        
        var rabbit = new AnimalType
        {
            Name = "Rabbit",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 1000,
            VisitorSatisfaction = 20
        };

        await CreateAnimalType(rabbit);
        
        var game = await CreateGame("asd");
        _ = await CreateAnimal(game, rabbit);
    }
    
    [Test]
    public async Task CreateAnimalMultiple()
    {
        await GameService.PruneDatabase(null);
        
        var rabbit = new AnimalType
        {
            Name = "Rabbit",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 900,
            VisitorSatisfaction = 20
        };

        await CreateAnimalType(rabbit);
        
        var game = await CreateGame("asd");

        foreach (var unused in ..100)
        {
            _ = await CreateAnimal(game, rabbit); 
        }
    }

    [Test]
    public async Task GetAllAnimals()
    {
        await GameService.PruneDatabase(null);
        
        var rabbit = new AnimalType
        {
            Name = "Rabbit",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 900,
            VisitorSatisfaction = 20
        };

        await CreateAnimalType(rabbit);
        
        var game = await CreateGame("asd");

        foreach (var unused in ..100)
        {
            _ = await CreateAnimal(game, rabbit); 
        }

        await using var db = await GameService.CreateDbContextAsync();
        var animals = GameService.AnimalService.GetAnimals(db, game);
        foreach (var animal in animals)
        {
            Assert.That(animal.AnimalTypeId, Is.EqualTo(rabbit.Id));
            Assert.That(animal.Health, Is.EqualTo(100));
            Assert.That(animal.HasChip, Is.False);
        }
    }

    [Test]
    public async Task TestAnimalUpdate()
    {
        await GameService.PruneDatabase(null);
        
        var rabbit = new AnimalType
        {
            Name = "Rabbit",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 900,
            VisitorSatisfaction = 20
        };

        await CreateAnimalType(rabbit);
        
        var game = await CreateGame("asd");
        var rabbitInstance = await CreateAnimal(game, rabbit);

        rabbitInstance.HasChip = true;
        rabbitInstance.Health = 90;
        rabbitInstance.HungerLevel = 10;
        rabbitInstance.ThirstLevel = 16;
        
        await using var db = await GameService.CreateDbContextAsync();
        var update = await GameService.AnimalService.UpdateAnimal(db, rabbitInstance);
        Assert.That(update.IsNone, Is.True);
        
        var newRabbit = await GameService.AnimalService.GetAnimalById(db, rabbitInstance.Id);
        Assert.That(newRabbit.HasValue, Is.True);
        var r = newRabbit.GetValueOrThrow();
        Assert.That(r, Is.EqualTo(rabbitInstance));
    }

    [Test]
    public async Task TestAnimalDiscoveries()
    {
        await GameService.PruneDatabase(null);
        
        var rabbit = new AnimalType
        {
            Name = "Rabbit",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 1000,
            VisitorSatisfaction = 20
        };

        var lake = new MapObjectType
        {
            Name = "Lake",
            ResourceType = ResourceType.Water,
            ResourceAmount = 1000,
            Price = 100
        };

        await using (var db = await GameService.CreateDbContextAsync())
        {
            var result1 = await GameService.AnimalService.CreateAnimalType(db, rabbit);
            Assert.That(result1.IsSome, Is.False);
            
            var result2 = await GameService.MapObjectService.CreateMapObjectType(db, lake);
            Assert.That(result2.IsSome, Is.False);
        }

        var game = await CreateGame("asd");
        
        var rabbit1 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        var rabbit2 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        var rabbit3 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        
        var lake1 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();
        var lake2 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();
        var lake3 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();

        await using (var db = await GameService.CreateDbContextAsync())
        {
            {
                var rabbitKnown1 = await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit1);
                Assert.That(rabbitKnown1.Count, Is.EqualTo(0));
                var rabbitKnown2 = await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit2);
                Assert.That(rabbitKnown2.Count, Is.EqualTo(0));
                var rabbitKnown3 = await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit3);
                Assert.That(rabbitKnown3.Count, Is.EqualTo(0));
            }

            var disc1 = await GameService.AnimalService.DiscoverMapObject(db, rabbit1, lake1);
            Assert.That(disc1.IsSome, Is.False);
            var disc2 = await GameService.AnimalService.DiscoverMapObject(db, rabbit2, lake1);
            Assert.That(disc2.IsSome, Is.False);
            var disc3 = await GameService.AnimalService.DiscoverMapObject(db, rabbit2, lake2);
            Assert.That(disc3.IsSome, Is.False);

            {
                var rabbitKnown1 = (await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit1)).ToList();
                Assert.That(rabbitKnown1.Count, Is.EqualTo(1));
                Assert.That(rabbitKnown1.First().Id, Is.EqualTo(lake1.Id));
                var rabbitKnown2 = (await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit2)).ToList();
                Assert.That(rabbitKnown2.Count, Is.EqualTo(2));
                Assert.That(rabbitKnown2[0].Id, Is.EqualTo(lake1.Id));
                Assert.That(rabbitKnown2[1].Id, Is.EqualTo(lake2.Id));
                var rabbitKnown3 = await GameService.AnimalService.GetDiscoveredMapObjects(db, rabbit3);
                Assert.That(rabbitKnown3.Count, Is.EqualTo(0));
            }

            await db.Entry(lake3).Collection(o=> o.DiscoveredByAnimals).LoadAsync();
            Assert.That(lake3.DiscoveredByAnimals.Count, Is.EqualTo(0));
        }
    }

    private async Task<Animal> CreateAnimal(SavedGame game, AnimalType animalType)
    {
        var purchaseResult = await GameService.PurchaseAnimal(game, animalType);
        Assert.That(purchaseResult, Is.Not.Null);
        Assert.That(purchaseResult.HasValue, Is.True);
        
        var animal = purchaseResult.GetValueOrThrow();
        Assert.That(animal.AnimalTypeId, Is.EqualTo(animalType.Id));

        return animal;
    }
    
    private async Task CreateAnimalType(AnimalType type)
    {
        await using var db = await GameService.CreateDbContextAsync();
        var result = await GameService.AnimalService.CreateAnimalType(db, type);
        Assert.That(result.IsSome, Is.False);
    }
}