using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Test.Unit;

public class PositionTests : UnitTestBase
{
    [Test]
    public async Task CreatePositionWithoutRoute()
    {
        await GameService.PruneDatabase(null);

        var type = new Position()
        {
            JeepRouteId = null,
            X = Random.Shared.NextDouble(),
            Y = Random.Shared.NextDouble()
        };
        
        var type2 = new Position()
        {
            JeepRouteId = null,
            X = type.X,
            Y = type.Y
        };

        var all = await GameService.GetPositions();
        Assert.That(all.Count(), Is.EqualTo(0));
        
        var result = await GameService.CreatePosition(type);
        Assert.That(result.IsNone, Is.True);

        all = (await GameService.GetPositions()).ToList();
        Assert.That(all.Count, Is.EqualTo(1));
        Assert.That(all, Is.EquivalentTo(new[] { type }));
        
        var result2 = await GameService.CreatePosition(type2);
        Assert.That(result2.IsNone, Is.True);
        
        all = (await GameService.GetPositions()).ToList();
        Assert.That(all.Count, Is.EqualTo(2));
        Assert.That(all, Is.EquivalentTo(new[] { type, type2 }));
    }
}