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

        var animals = await GameService.GetAnimals(game);
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
        
        var update = await GameService.UpdateAnimal(rabbitInstance);
        Assert.That(update.IsNone, Is.True);
        
        var newRabbit = await GameService.GetAnimalById(rabbitInstance.Id);
        Assert.That(newRabbit.HasValue, Is.True);
        var r = newRabbit.GetValueOrThrow();
        Assert.That(r.HasChip, Is.True);
        Assert.That(r.Health, Is.EqualTo(90));
        Assert.That(r.HungerLevel, Is.EqualTo(10));
        Assert.That(r.ThirstLevel, Is.EqualTo(16));
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

        {
            var result1 = await GameService.CreateAnimalType(rabbit);
            Assert.That(result1.IsSome, Is.False);
            
            var result2 = await GameService.CreateMapObjectType(lake);
            Assert.That(result2.IsSome, Is.False);
        }

        var game = await CreateGame("asd");
        
        var rabbit1 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        var rabbit2 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        var rabbit3 = (await GameService.PurchaseAnimal(game, rabbit)).GetValueOrThrow();
        
        var lake1 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();
        var lake2 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();
        var lake3 = (await GameService.PurchaseMapObject(game, lake)).GetValueOrThrow();

        {
            {
                var rabbitKnown1 = await GameService.GetDiscoveredMapObjects(rabbit1);
                Assert.That(rabbitKnown1.Count, Is.EqualTo(0));
                var rabbitKnown2 = await GameService.GetDiscoveredMapObjects(rabbit2);
                Assert.That(rabbitKnown2.Count, Is.EqualTo(0));
                var rabbitKnown3 = await GameService.GetDiscoveredMapObjects(rabbit3);
                Assert.That(rabbitKnown3.Count, Is.EqualTo(0));
            }

            var disc1 = await GameService.DiscoverMapObject(rabbit1, lake1);
            Assert.That(disc1.IsSome, Is.False);
            var disc2 = await GameService.DiscoverMapObject(rabbit2, lake1);
            Assert.That(disc2.IsSome, Is.False);
            var disc3 = await GameService.DiscoverMapObject(rabbit2, lake2);
            Assert.That(disc3.IsSome, Is.False);

            {
                var rabbitKnown1 = (await GameService.GetDiscoveredMapObjects(rabbit1)).ToList();
                Assert.That(rabbitKnown1.Count, Is.EqualTo(1));
                Assert.That(rabbitKnown1.First().Id, Is.EqualTo(lake1.Id));
                var rabbitKnown2 = (await GameService.GetDiscoveredMapObjects(rabbit2)).ToList();
                Assert.That(rabbitKnown2.Count, Is.EqualTo(2));
                Assert.That(rabbitKnown2[0].Id, Is.EqualTo(lake1.Id));
                Assert.That(rabbitKnown2[1].Id, Is.EqualTo(lake2.Id));
                var rabbitKnown3 = await GameService.GetDiscoveredMapObjects(rabbit3);
                Assert.That(rabbitKnown3.Count, Is.EqualTo(0));
            }

            await GameService.LoadCollection(lake3, o => o.DiscoveredByAnimals);
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
        var result = await GameService.CreateAnimalType(type);
        Assert.That(result.IsSome, Is.False);
    }
}