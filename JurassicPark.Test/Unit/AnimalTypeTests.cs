using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Test.Unit;

[TestFixture]
public class AnimalTypeTests : UnitTestBase
{
    [Test]
    public async Task CreateAnimalTypeAndEnsureUnique()
    {
        await GameService.PruneDatabase(null);

        var type = new AnimalType
        {
            Name = "Some animal",
            EatingHabit = AnimalEatingHabit.Herbivore,
            Price = 100,
            VisitorSatisfaction = 100
        };
        
        await using var db = await GameService.CreateDbContextAsync();

        var all = GameService.AnimalService.GetAllAnimalTypes(db);
        Assert.That(all.Count(), Is.EqualTo(0));
        
        var result = await GameService.AnimalService.CreateAnimalType(db, type);
        Assert.That(result.IsNone, Is.True);

        all = GameService.AnimalService.GetAllAnimalTypes(db).ToList();
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all, Is.EquivalentTo(new[] { type }));
        
        var result2 = await GameService.AnimalService.CreateAnimalType(db, type);
        Assert.That(result2.IsSome, Is.True);
        var errorType = result2.AsSome.Value;
        Assert.That(errorType, Is.TypeOf<ConflictError>());

        all = GameService.AnimalService.GetAllAnimalTypes(db).ToList();
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all, Is.EquivalentTo(new[] { type }));
    }
}