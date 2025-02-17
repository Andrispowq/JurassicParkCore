using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Test.Unit;

public abstract class GameRequiredTest : UnitTestBase
{
    public async Task<SavedGame> CreateGame(string name,
        Difficulty difficulty = Difficulty.Easy, long width = 1000, long height = 1000)
    {
        var result = await GameService.CreateNewGame(name, difficulty, width, height);
        if (result.IsError) Assert.Fail("Game could not be created");
        return result.GetValueOrThrow();
    }

    public async Task DeleteGame(SavedGame game)
    {
        var success = await GameService.PruneDatabase(game);
        if (!success) Assert.Fail("Game could not be pruned");
    }
}