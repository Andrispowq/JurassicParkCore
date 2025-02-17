using JurassicPark.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Test.Unit;

public abstract class UnitTestBase
{
    protected IGameService GameService;

    [SetUp]
    public void Setup()
    {
        var scope = GlobalTestSetup.ServiceProvider?.CreateScope();
        GameService = scope?.ServiceProvider.GetRequiredService<IGameService>() 
                       ?? throw new NullReferenceException();
    }
    
    [TearDown]
    public async Task Cleanup()
    {
        await GameService.PruneDatabase(null);
    }
}