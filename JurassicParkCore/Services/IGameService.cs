using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services;

public interface IGameService
{
    IAnimalService AnimalService { get; }
    IPositionService PositionService { get; }
    ITransactionService TransactionService { get; }
    
    Task<JurassicParkDbContext> GetDbContextAsync();
    
    Task<Result<SavedGame, ServiceError>> CreateNewGame(Difficulty difficulty, long width, long height);
    Task<Result<Animal, ServiceError>> PurchaseAnimal(SavedGame game, AnimalType type);
}