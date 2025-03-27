using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services;
using JurassicPark.Core.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Test.Integration;

[TestFixture]
public class BasicGameTest
{
    private readonly AnimalType _trex = new()
    {
        Name = "Tyrannosaurus Rex",
        EatingHabit = AnimalEatingHabit.Carnivore,
        Price = 2000,
        VisitorSatisfaction = 15
    };
    
    private readonly MapObjectType _stone = new()
    {
        Name = "Stone",
        Price = 100,
        ResourceType = ResourceType.Other,
        ResourceAmount = 0,
    };
    private readonly MapObjectType _grass = new()
    {
        Name = "Grass",
        Price = 10,
        ResourceType = ResourceType.Vegetation,
        ResourceAmount = 50,
    };
    private readonly MapObjectType _mediumLake = new()
    {
        Name = "Medium Lake",
        Price = 500,
        ResourceType = ResourceType.Water,
        ResourceAmount = 1000,
    };
    
    private IGameService _gameService;

    [SetUp]
    public void Setup()
    {
        var scope = GlobalTestSetup.ServiceProvider?.CreateScope();
        _gameService = scope?.ServiceProvider.GetRequiredService<IGameService>() 
                       ?? throw new NullReferenceException();
    }

    [TearDown]
    public async Task Cleanup()
    {
        await _gameService.PruneDatabase(null);
    }
    
    [Test]
    public async Task SetupGameElements()
    {
        await _gameService.PruneDatabase(null);
        await _gameService.InitialiseComponents([_trex], [_stone, _grass, _mediumLake]);
        
        var animalTypes = (await _gameService.GetAnimalTypes())
            .OrderBy(t => t.Id);
        var expectedAnimals = new List<AnimalType> { _trex }
            .OrderBy(t => t.Id);
        Assert.That(animalTypes, Is.EqualTo(expectedAnimals));
            
        var objectTypes = (await _gameService.GetMapObjectTypes())
            .OrderBy(t => t.Id);
        var expectedObjects = new List<MapObjectType> { _stone, _grass, _mediumLake }
            .OrderBy(t => t.Id);
        Assert.That(objectTypes, Is.EqualTo(expectedObjects));
    }

    [Test]
    public async Task Lose()
    {
        await SetupGameElements();
        
        var gameResult = await _gameService.CreateNewGame("loseGame", Difficulty.Medium, 1000, 1000);
        Assert.That(gameResult.HasValue, Is.True);

        var game = gameResult.GetValueOrThrow();
        Assert.That(game.Name, Is.EqualTo("loseGame"));
        Assert.That(game.Difficulty, Is.EqualTo(Difficulty.Medium));
        Assert.That(game.MapWidth, Is.EqualTo(1000));
        Assert.That(game.MapHeight, Is.EqualTo(1000));
        Assert.That(game.TimeOfDay, Is.EqualTo(GameService.StartTime));
        Assert.That(game.DaysPassed, Is.EqualTo(0));
        Assert.That(game.VisitorSatisfaction, Is.EqualTo(0));
        Assert.That(game.HoursSinceGoalMet, Is.EqualTo(0));
        Assert.That(game.GameState, Is.EqualTo(GameState.Ongoing));
        Assert.That(game.GameSpeed, Is.EqualTo(GameSpeed.Moderate));
        
        var position = new Position
        {
            X = 1,
            Y = 2,
        };
        var positionResult  = await _gameService.CreatePosition(position);
        Assert.That(positionResult.IsNone, Is.True);
        
        {
            var currentBalanceResult = await _gameService.GetCurrentBalance(game);
            Assert.That(currentBalanceResult.HasValue, Is.True);
            
            var currentBalance = currentBalanceResult.GetValueOrThrow();
            Assert.That(currentBalance, Is.EqualTo(GameService.InitialMoney));
        }

        for (int i = 0; i < GameService.InitialMoney / _trex.Price; i++)
        {
            var purchaseResult = await _gameService.PurchaseAnimal(game, _trex, position);
            Assert.That(purchaseResult.HasValue, Is.True);
        
            var animal = purchaseResult.GetValueOrThrow();
            Assert.That(animal.AnimalTypeId, Is.EqualTo(_trex.Id));
        }
        
        //Assert that we cannot go into negatives
        var finalPurchaseResult = await _gameService.PurchaseAnimal(game, _trex, position);
        Assert.That(finalPurchaseResult.IsError, Is.True);
        
        var error = finalPurchaseResult.GetErrorOrThrow();
        Assert.That(error, Is.TypeOf<UnauthorizedError>());
        
        Assert.That(game.GameState, Is.EqualTo(GameState.Ongoing));
        
        //Let's now move us into the negatives, losing the game
        {
            var transaction = new Transaction
            {
                Type = TransactionType.Purchase,
                Amount = GameService.InitialMoney,
                SavedGameId = game.Id
            };
            
            var currentBalanceResult = await _gameService.CreateTransaction(game, transaction, true);
            Assert.That(currentBalanceResult.IsSome, Is.True);
        }
        
        Assert.That(game.GameState, Is.EqualTo(GameState.Lost));
    }

    [Test]
    public async Task Perform()
    {
        await SetupGameElements();

        var gameResult = await _gameService.CreateNewGame("performGame", Difficulty.Medium, 1000, 1000);
        Assert.That(gameResult.HasValue, Is.True);

        var game = gameResult.GetValueOrThrow();
        Assert.That(game.Name, Is.EqualTo("performGame"));
        Assert.That(game.Difficulty, Is.EqualTo(Difficulty.Medium));
        Assert.That(game.MapWidth, Is.EqualTo(1000));
        Assert.That(game.MapHeight, Is.EqualTo(1000));
        Assert.That(game.TimeOfDay, Is.EqualTo(GameService.StartTime));
        Assert.That(game.DaysPassed, Is.EqualTo(0));
        Assert.That(game.VisitorSatisfaction, Is.EqualTo(0));
        Assert.That(game.HoursSinceGoalMet, Is.EqualTo(0));
        Assert.That(game.GameState, Is.EqualTo(GameState.Ongoing));
        Assert.That(game.GameSpeed, Is.EqualTo(GameSpeed.Moderate));

        {
            var currentBalanceResult = await _gameService.GetCurrentBalance(game);
            Assert.That(currentBalanceResult.HasValue, Is.True);

            var currentBalance = currentBalanceResult.GetValueOrThrow();
            Assert.That(currentBalance, Is.EqualTo(GameService.InitialMoney));
        }
        
        var position = new Position
        {
            X = 1,
            Y = 2,
        };
        var positionResult  = await _gameService.CreatePosition(position);
        Assert.That(positionResult.IsNone, Is.True);

        var purchaseResult = await _gameService.PurchaseAnimal(game, _trex, position);
        Assert.That(purchaseResult.HasValue, Is.True);

        var animal = purchaseResult.GetValueOrThrow();
        Assert.That(animal.AnimalTypeId, Is.EqualTo(_trex.Id));

        {
            var currentBalanceResult = await _gameService.GetCurrentBalance(game);
            Assert.That(currentBalanceResult.HasValue, Is.True);

            var currentBalance = currentBalanceResult.GetValueOrThrow();
            Assert.That(currentBalance, Is.EqualTo(GameService.InitialMoney - _trex.Price));
        }
    }
}