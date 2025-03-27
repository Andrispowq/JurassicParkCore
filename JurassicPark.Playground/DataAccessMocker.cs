using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicParkTester;

public class DataAccessMocker(IGameService gameService)
{
    public async Task Run()
    {
        while (true)
        {
            var input = Console.ReadLine();
            if (input is null) return;

            input = input.Trim().ToLower();
            if (input == "quit")
            {
                break;
            }

            if (input.StartsWith("prune "))
            {
                var gameName = input.Substring("prune ".Length);
                var game = await gameService.GetSavedGameByName(gameName);
                if (game.IsError)
                {
                    Console.WriteLine("Game not found");
                    continue;
                }

                await gameService.PruneDatabase(game.GetValueOrThrow());
            }

            if (input == "run")
            {
                await MockGame();
            }
        }
    }

    private async Task MockGame()
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

        string? gameName;
        while (true)
        {
            Console.Write("Enter a name for your save: ");
            gameName = Console.ReadLine();

            if (gameName is not null)
            {
                var gameFound = await gameService.GetSavedGameByName(gameName);
                if (gameFound.IsError) //If the game is not found, we can create it
                {
                    break;
                }
            }
        }

        var position = new Position
        {
            X = 1,
            Y = 2
        };
        
        (await gameService.CreatePosition(position)).ThrowIfSome();

        var game = (await gameService.CreateNewGame(gameName, Difficulty.Medium, 1000, 1000)).GetValueOrThrow();
        (await gameService.PurchaseAnimal(game, trex, position)).GetValueOrThrow();
    }
}