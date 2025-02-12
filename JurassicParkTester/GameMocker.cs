using JurassicParkCore.DataSchemas;
using JurassicParkCore.Services;

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
        
        await using (var db = await gameService.GetDbContextAsync())
        {
            (await gameService.AnimalService.CreateAnimalType(db, trex)).ThrowIfSome();

            var position = new Position() { X = 1000, Y = 1000 };
            (await gameService.PositionService.CreatePosition(db, position)).ThrowIfSome();
        }

        var game = (await gameService.CreateNewGame(Difficulty.Medium, 1000, 1000)).GetValueOrThrow();
        (await gameService.PurchaseAnimal(game, trex)).GetValueOrThrow();
    }
}