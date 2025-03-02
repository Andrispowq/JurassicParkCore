using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Model;

public static class HerbivoreBehaviour
{
    public static async Task UpdateStats(this Animal animal)
    {
        await Task.CompletedTask;
    }
}

public class JurassicParkModel(IGameService gameService)
{
    public event EventHandler<SavedGame>? GameCreated; 
    public event EventHandler<SavedGame>? GameNearLost;
    public event EventHandler<SavedGame>? GameGoalMet;
    public event EventHandler<(SavedGame, bool)>? GameOver;

    private SavedGame? _savedGame;
    public IEnumerable<Animal> Animals { get; private set; }

    public async Task SaveAsync()
    {
        if (_savedGame is null)
        {
            return;
        }
        
        foreach (var animal in Animals)
        {
            await animal.UpdateStats();
            //await gameService.UpdateAnimal(animal);
        }
        
        await gameService.UpdateGame(_savedGame);
    }
    
    public async Task UpdateAsync(double delta)
    {
        await Task.CompletedTask;
    }  
}