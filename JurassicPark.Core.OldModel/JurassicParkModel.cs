using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using JurassicPark.Core.OldModel.Behaviours;
using JurassicPark.Core.OldModel.Connection;
using JurassicPark.Core.OldModel.Dto;
using JurassicPark.Core.OldModel.Functional;
using JurassicPark.Core.OldModel.Validators;

namespace JurassicPark.Core.OldModel
{
    public class JurassicParkModel : IJurassicParkModel
    {
        public decimal JeepPrice => 1000;
        public decimal RouteBlockPrice => 50;

        public List<Animal> Animals { get; private set; } = null!;
        public List<AnimalGroup> AnimalGroups { get; private set; } = null!;
        public List<AnimalType> AnimalTypes { get; private set; } = null!;
        public List<Jeep> Jeeps { get; private set; } = null!;
        public List<JeepRoute> JeepRoutes { get; private set; } = null!;
        public List<MapObject> MapObjects { get; private set; } = null!;
        public List<MapObjectType> MapObjectTypes { get; private set; } = null!;
        public List<Position> Positions { get; private set; } = null!;
        public List<Transaction> Transactions { get; private set; } = null!;

        public SavedGame? SavedGame { get; private set; }

        private readonly Connection.Connection _connection = new Connection.Connection();
        
        private readonly IAnimalBehaviourHandler _animalBehaviourHandler;
        private readonly IRouteValidator _routeValidator;

        public JurassicParkModel(IAnimalBehaviourHandler animalBehaviourHandler,
            IRouteValidator routeValidator)
        {
            _animalBehaviourHandler = animalBehaviourHandler;
            _routeValidator = routeValidator;
            
            //For Parse and TryParse to work correctly
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentUICulture = CultureInfo.InvariantCulture;
        }

        public async Task<IEnumerable<SavedGame>> LoadSavedGamesAsync()
        {
            return await _connection.RequestList<SavedGame>(new GetRequest("games"));
        }

        public async Task<Option<ServiceError>> LoadGameAsync(long id)
        {
            if (SavedGame != null)
                return new ConflictError("Game already loaded");
            
            var game = await _connection.Request<SavedGame>(new GetRequest($"games/{id}"));
            if (game is null)
            {
                return new NotFoundError("Game not found");
            }

            SavedGame = game;

            Animals = await _connection.RequestList<Animal>(new GetRequest($"games/{id}/animals"));
            AnimalGroups = await _connection.RequestList<AnimalGroup>(new GetRequest($"games/{id}/animal-groups"));
            AnimalTypes = await _connection.RequestList<AnimalType>(new GetRequest("animal-types"));
            Jeeps = await _connection.RequestList<Jeep>(new GetRequest($"games/{id}/jeeps"));
            JeepRoutes = await _connection.RequestList<JeepRoute>(new GetRequest($"games/{id}/jeep-routes"));
            MapObjects = await _connection.RequestList<MapObject>(new GetRequest($"games/{id}/map-objects"));
            MapObjectTypes = await _connection.RequestList<MapObjectType>(new GetRequest($"map-object-types"));
            Positions = await _connection.RequestList<Position>(new GetRequest($"positions"));
            Transactions = await _connection.RequestList<Transaction>(new GetRequest($"games/{id}/transactions"));

            return new Option<ServiceError>.None();
        }

        public async Task<Option<ServiceError>> UnloadGameAsync()
        {
            if (SavedGame is null)
                return new NotFoundError("Game not found");

            await SaveAsync();
            
            Animals.Clear();
            AnimalGroups.Clear();
            Jeeps.Clear();
            JeepRoutes.Clear();
            MapObjects.Clear();
            Transactions.Clear();

            SavedGame = null;
            return new Option<ServiceError>.None();
        }

        public async Task<Result<SavedGame, ServiceError>> CreateGameAsync(string name,
            Difficulty difficulty, long mapWidth, long mapHeight)
        {
            if (SavedGame != null)
                return new ConflictError("Game already exists");

            var request = new SavedGameCreateDto
            {
                Difficulty = difficulty,
                MapWidth = mapWidth,
                MapHeight = mapHeight,
                Name = name,
            };
            
            var game = await _connection.Request<SavedGame>(new PostRequest("games", request));
            if (game != null)
            {
                //SavedGame = game;
                return game;
            }

            return new ConflictError("Game could not be created");
        }

        public async Task<Option<ServiceError>> DeleteGameAsync()
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            Animals.Clear();
            AnimalGroups.Clear();
            Jeeps.Clear();
            JeepRoutes.Clear();
            MapObjects.Clear();
            Transactions.Clear();

            var result = await _connection.Request<string>(new DeleteRequest($"games/{SavedGame.Id}", null));
            SavedGame = null;

            if (result != null)
            {
                return new NotFoundError("Game not found");
            }
            
            return new Option<ServiceError>.None();
        }

        public async Task SaveAsync()
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return;

            foreach (var animal in Animals)
            {
                var updated = new AnimalUpdateRequest
                {
                    Age = animal.Age,
                    HasChip = animal.HasChip,
                    State = animal.State,
                    HungerLevel = animal.HungerLevel,
                    ThirstLevel = animal.ThirstLevel,
                    Health = animal.Health,
                    Position = new CreatePositionDto
                    {
                        X = animal.Position.X,
                        Y = animal.Position.Y
                    }
                };
                
                await _connection.Request<string>(new PutRequest($"games/{SavedGame.Id}/animals/{animal.Id}", updated));
            }

            /*foreach (var group in AnimalGroups)
            {
                await gameService.UpdateGroup(group);
            }

            foreach (var jeep in Jeeps)
            {
                await gameService.UpdateJeep(jeep);
            }

            foreach (var jeepRoute in JeepRoutes)
            {
                await gameService.UpdateRoute(jeepRoute);
            }*/

            foreach (var mapObject in MapObjects)
            {
                var updated = new MapObjectUpdateRequest
                {
                    ResourceAmount = mapObject.ResourceAmount
                };
                
                await _connection.Request<string>(new PutRequest($"games/{SavedGame.Id}/map-object/{mapObject.Id}", updated));
            }

            foreach (var position in Positions)
            {
                var updated = new CreatePositionDto
                {
                    X = position.X,
                    Y = position.Y
                };

                await _connection.Request<string>(new PutRequest($"positions/{position.Id}",
                    updated));
            }

            var updatedGame = new SavedGameUpdateRequest
            {
                Difficulty = SavedGame.Difficulty,
                TimeOfDay = SavedGame.TimeOfDay,
                DaysPassed = SavedGame.DaysPassed,
                VisitorSatisfaction = SavedGame.VisitorSatisfaction,
                HoursSinceGoalMet = SavedGame.HoursSinceGoalMet,
                GameState = SavedGame.GameState,
                GameSpeed = SavedGame.GameSpeed
            };

            await _connection.Request<string>(new PutRequest($"games/{SavedGame.Id}",
                updatedGame));
        }

        public async Task<IEnumerable<AnimalType>> GetAnimalTypes()
        {
            AnimalTypes = await _connection.RequestList<AnimalType>(new GetRequest("animal-types"));
            return AnimalTypes;
        }

        public async Task<Result<AnimalType, ServiceError>> CreateAnimalType(CreateAnimalTypeRequest request)
        {
            var obj = await _connection.Request<AnimalType>(new PostRequest("animal-types", request));
            if (obj is null) return new ConflictError("Could not create animal type");

            AnimalTypes.Add(obj);
            return obj;
        }

        public async Task<IEnumerable<MapObjectType>> GetMapObjectTypes()
        {
            MapObjectTypes = await _connection.RequestList<MapObjectType>(new GetRequest("map-object-types"));
            return MapObjectTypes;
        }

        public async Task<Result<MapObjectType, ServiceError>> CreateMapObjectType(CreateMapObjectTypeDto request)
        {
            var obj = await _connection.Request<MapObjectType>(new PostRequest("map-object-types", request));
            if (obj is null) return new ConflictError("Could not create map object type");

            MapObjectTypes.Add(obj);
            return obj;
        }

        public async Task<Result<decimal, ServiceError>> GetBalance()
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var result =
                await _connection.Request<string>(new GetRequest($"games/{SavedGame.Id}/transactions/balance"));
            if (result is null) return new NotFoundError("Game not found");

            if (decimal.TryParse(result, out var res))
            {
                return res;
            }
            
            return new NotFoundError("Game not found");
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return new List<Transaction>();
            
            Transactions = await _connection.RequestList<Transaction>(new GetRequest($"games/{SavedGame.Id}/transactions"));
            return Transactions;
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return new List<Transaction>();
            
            return await _connection.RequestList<Transaction>(new GetRequest($"games/{SavedGame.Id}/transactions/all"));
        }

        public async Task<Result<Transaction, ServiceError>> CreateCheckpoint()
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return new NotFoundError("Game is already over");

            var result =
                await _connection.Request<Transaction>(new PostRequest($"games/{SavedGame.Id}/transactions/create-checkpoint", null));
            if (result is null) return new NotFoundError("Game not found");

            return result;
        }

        public async Task<Result<Transaction, ServiceError>> CreateTransaction(TransactionType type, decimal amount, bool canLose)
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return new NotFoundError("Game is already over");

            if (type == TransactionType.Checkpoint)
            {
                return new UnauthorizedError("Can not create checkpoint with this method");
            }

            var request = new CreateTransactionDto
            {
                Type = type,
                Amount = amount,
                CanLose = canLose
            };

            var result = await _connection.Request<Transaction>(
                new PostRequest($"games/{SavedGame.Id}/transactions", request));
            if (result is null) return new NotFoundError("Game not found");

            return result;
        }

        private async Task UpdateTransactions()
        {
            var list = await _connection.Request<List<Transaction>>(new GetRequest($"games/{SavedGame?.Id}/transactions"));
            if (list is null) return;

            Transactions = list;
        }
        
        public async Task<Result<Animal, ServiceError>> PurchaseAnimal(AnimalType animalType, Position position)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var animal = await _connection.Request<Animal>(new PostRequest($"games/{SavedGame.Id}/animals/purchase/{animalType.Id}", 
                position));
            await UpdateTransactions();
            
            if (animal != null)
            {
                Animals.Add(animal);
                return animal;
            }

            return new NotFoundError("Animal could not be purchased");
        }

        public async Task<Option<ServiceError>> SellAnimal(Animal animal)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var original = animal.AnimalType.Price;
            var age = animal.Age;
            var refundPrice = original * (decimal)Math.Cbrt(1.0 / (age + 1));

            Animals.Remove(animal);
            
            var result = await _connection.Request<string>(new DeleteRequest($"games/{SavedGame.Id}/animals/{animal.Id}/sell", 
                refundPrice));
            await UpdateTransactions();
            
            if (result == null)
            {
                return new NotFoundError("Animal not found");
            }
            
            return new Option<ServiceError>.None();
        }

        public async Task<Option<ServiceError>> KillAnimal(Animal animal)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            Animals.Remove(animal);
            
            var result = await _connection.Request<string>(new DeleteRequest($"games/{SavedGame.Id}/animals/{animal.Id}/kill", null));
            if (result == null)
            {
                return new NotFoundError("Animal not found");
            }
            
            return new Option<ServiceError>.None();
        }

        public async Task<Result<MapObject, ServiceError>> PurchaseMapObject(MapObjectType mapObjectType,
            Position position)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var obj = await _connection.Request<MapObject>(new PostRequest($"games/{SavedGame.Id}/map-objects/purchase/{mapObjectType.Id}", 
                position));
            await UpdateTransactions();
            
            if (obj == null) return new ConflictError("MapObject could not be purchased");
            
            MapObjects.Add(obj);
            return obj;
        }

        public async Task<Option<ServiceError>> SellMapObject(MapObject mapObject)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            //Calculate refund by lerping
            var original = mapObject.MapObjectType.Price;
            var resourcePercent = mapObject.ResourceAmount / mapObject.MapObjectType.ResourceAmount;
            var refundPrice = original * resourcePercent;

            MapObjects.Remove(mapObject);
            
            var result = await _connection.Request<string>(new DeleteRequest($"games/{SavedGame.Id}/map-objects/{mapObject.Id}/sell", 
                refundPrice));
            await UpdateTransactions();
            
            if (result == null)
            {
                return new NotFoundError("MapObject not found");
            }
            
            return new Option<ServiceError>.None();
        }

        public async Task<Option<ServiceError>> RemoveMapObject(MapObject mapObject)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            MapObjects.Remove(mapObject);
            
            var result = await _connection.Request<string>(new DeleteRequest($"games/{SavedGame.Id}/map-objects/{mapObject.Id}/kill", null));
            if (result == null)
            {
                return new NotFoundError("MapObject not found");
            }
            
            return new Option<ServiceError>.None();
        }

        public async Task<Result<Jeep, ServiceError>> PurchaseJeep()
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");
            
            var obj = await _connection.Request<Jeep>(new PostRequest($"games/{SavedGame.Id}/jeep", JeepPrice));
            if (obj == null) return new ConflictError("Jeep could not be purchased");
            
            await UpdateTransactions();
            
            Jeeps.Add(obj);
            return obj;
        }

        public async Task<Option<ServiceError>> SellJeep(Jeep jeep)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            Jeeps.Remove(jeep);

            var refundPrice = JeepPrice * 0.75m;
            
            var result = await _connection.Request<Jeep>(new DeleteRequest($"games/{SavedGame.Id}/jeep/{jeep.Id}/sell", 
                refundPrice));
            await UpdateTransactions();
            
            if (result is null) return new NotFoundError("Jeep not found");
            
            return new Option<ServiceError>.None();
        }

        public async Task<Option<ServiceError>> DuplicateRoute(JeepRoute jeepRoute, Position position)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var contains = _routeValidator.IsPositionOnRoute(jeepRoute, position);
            if (!contains) return new BadRequestError("Route doesn't have this position");

            //TODO
            throw new NotImplementedException();
        }

        public async Task<Option<ServiceError>> PurchaseRoadBlock(JeepRoute jeepRoute, Position position)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var canPurchase = _routeValidator.CanAddRouteBlock(jeepRoute, position);
            if (!canPurchase)
            {
                return new BadRequestError("Can not remove any more elements");
            }

            await UpdateTransactions();
            
            //TODO
            return new Option<ServiceError>.None();
        }

        public async Task<Option<ServiceError>> SellRoadBlock(JeepRoute jeepRoute)
        {
            if (SavedGame is null)
                return new NotFoundError("No game active");
            if (SavedGame.GameState != GameState.Ongoing)
                return new UnauthorizedError("Game is already over");

            var canRemove = _routeValidator.CanRemoveElement(jeepRoute);
            if (canRemove)
            {
                //TODO remove
                //TODO is empty, delete
                await UpdateTransactions();
            
            }

            return new BadRequestError("Can not remove any more elements");
        }

        public async Task UpdateAsync(double delta)
        {
            if (SavedGame?.GameState != GameState.Ongoing)
                return;

            foreach (var animal in Animals)
            {
                await _animalBehaviourHandler.ApplySingleChangesAsync(this, animal, delta);
            }

            await _animalBehaviourHandler.ApplyGroupChangesAsync(this, delta);

            //throw new NotImplementedException();
        }
    }
}