using JurassicParkCore.DataSchemas;
using JurassicParkCore.Services.Interfaces;

namespace JurassicParkTester;

public class GameMocker(IGameService gameService)
{
    public async Task MockGame()
    {
        var trex = new AnimalType
        {
            Name = "Tyrannosaurus Rex",
            EatingHabit = AnimalEatingHabit.Carnivore,
            Price = 2000,
            VisitorSatisfaction = 15
        };

        var stone = new MapObjectType
        {
            Name = "Stone",
            Price = 100,
            ResourceType = ResourceType.Other,
            ResourceAmount = 0,
        };
        var grass = new MapObjectType
        {
            Name = "Grass",
            Price = 10,
            ResourceType = ResourceType.Vegetation,
            ResourceAmount = 50,
        };
        var mediumLake = new MapObjectType
        {
            Name = "Medium Lake",
            Price = 500,
            ResourceType = ResourceType.Water,
            ResourceAmount = 1000,
        };

        await gameService.InitialiseComponents([trex], [stone, grass, mediumLake]);

        Console.Write("Enter a name for your save: ");
        string gameName = Console.ReadLine() ?? "";
        var game = (await gameService.CreateNewGame(gameName, Difficulty.Medium, 1000, 1000)).GetValueOrThrow();
        (await gameService.PurchaseAnimal(game, trex)).GetValueOrThrow();
    }
}