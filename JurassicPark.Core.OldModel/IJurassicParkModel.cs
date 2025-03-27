using System.Collections.Generic;
using System.Threading.Tasks;
using JurassicPark.Core.OldModel.Dto;
using JurassicPark.Core.OldModel.Functional;

namespace JurassicPark.Core.OldModel
{
    public interface IJurassicParkModel
    {
        decimal JeepPrice { get; }
        decimal RouteBlockPrice { get; }

        //Game objects
        List<Animal> Animals { get; }
        List<AnimalGroup> AnimalGroups { get; }
        List<AnimalType> AnimalTypes { get; }
        //ist<Jeep> Jeeps { get; }
        //List<JeepRoute> JeepRoutes { get; }
        List<MapObject> MapObjects { get; }
        //List<MapObjectType> MapObjectTypes { get; }
        List<Position> Positions { get; }
        List<Transaction> Transactions { get; }
        SavedGame? SavedGame { get; }

        //Game stuff
        Task<IEnumerable<SavedGame>> LoadSavedGamesAsync();
        Task<Option<ServiceError>> LoadGameAsync(long id);

        Task<Result<SavedGame, ServiceError>> CreateGameAsync(string name, Difficulty difficulty, long mapWidth,
            long mapHeight);

        Task<Option<ServiceError>> DeleteGameAsync();

        //Update and save
        Task UpdateAsync(double delta);
        Task SaveAsync();

        //Transactional stuff
        Task<Result<Animal, ServiceError>> PurchaseAnimal(AnimalType animalType, Position position);
        Task<Option<ServiceError>> SellAnimal(Animal animal);
        Task<Option<ServiceError>> KillAnimal(Animal animal);

        Task<Result<MapObject, ServiceError>> PurchaseMapObject(MapObjectType mapObjectType, Position position);
        Task<Option<ServiceError>> SellMapObject(MapObject mapObject);
        Task<Option<ServiceError>> RemoveMapObject(MapObject mapObject);

        //Task<Result<Jeep, ServiceError>> PurchaseJeep();
        //Task<Option<ServiceError>> SellJeep(Jeep jeep);

        //Routes
        /// <summary>
        /// This method shall be called when an existing route has to be split into two
        /// </summary>
        /// <param name="jeepRoute">The route to be duplicated</param>
        /// <param name="position">The position of the last common point. It is at this position that the new route ends,
        /// giving way expansion</param>
        /// <returns>None if the method succeeded. Any ServiceError if some error occurs.</returns>
        //Task<Option<ServiceError>> DuplicateRoute(JeepRoute jeepRoute, Position position);

        /// <summary>
        /// This method shall be called when a new road block is placed down
        /// </summary>
        /// <param name="jeepRoute">The route upon which the new road block is placed down</param>
        /// <param name="position">The position of the new block. Coordinates are rounded down to the nearest integer</param>
        /// <returns>None if the method succeeded. Any ServiceError if some error occurs.</returns>
        //Task<Option<ServiceError>> PurchaseRoadBlock(JeepRoute jeepRoute, Position position);

        /// <summary>
        /// This method will remove the last element of the specified route
        /// </summary>
        /// <param name="jeepRoute">The route upon which the new road block is placed down</param>
        /// <returns>None if the method succeeded. NotFoundError if the route is empty. Any other ServiceError if some error occurred while executing the call.</returns>
        //Task<Option<ServiceError>> SellRoadBlock(JeepRoute jeepRoute);
    }
}