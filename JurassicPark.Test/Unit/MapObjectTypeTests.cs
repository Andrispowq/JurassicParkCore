using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Test.Unit;

public class MapObjectTypeTests : UnitTestBase
{
    [Test]
    public async Task CreateMapObjectTypeAndEnsureUnique()
    {
        await GameService.PruneDatabase(null);

        var type = new MapObjectType
        {
            Name = "Some object",
            Price = 100,
            ResourceType = ResourceType.Water,
            ResourceAmount = 0
        };
        
        await using var db = await GameService.CreateDbContextAsync();

        var all = GameService.MapObjectService.GetMapObjectTypes(db);
        Assert.That(all.Count(), Is.EqualTo(0));
        
        var result = await GameService.MapObjectService.CreateMapObjectType(db, type);
        Assert.That(result.IsNone, Is.True);

        all = GameService.MapObjectService.GetMapObjectTypes(db).ToList();
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all, Is.EquivalentTo(new[] { type }));
        
        var result2 = await GameService.MapObjectService.CreateMapObjectType(db, type);
        Assert.That(result2.IsSome, Is.True);
        var errorType = result2.AsSome.Value;
        Assert.That(errorType, Is.TypeOf<ConflictError>());

        all = GameService.MapObjectService.GetMapObjectTypes(db).ToList();
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all, Is.EquivalentTo(new[] { type }));
    }
}