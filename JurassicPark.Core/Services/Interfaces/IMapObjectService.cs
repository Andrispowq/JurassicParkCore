using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IMapObjectService
{
    //Types
    IEnumerable<MapObjectType> GetMapObjectTypes(JurassicParkDbContext context);
    Task<Result<MapObjectType, ServiceError>> GetMapObjectTypeById(JurassicParkDbContext context, long id);
    Task<Option<ServiceError>> CreateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType);
    Task<Option<ServiceError>> UpdateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType);
    Task<Option<ServiceError>> DeleteMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType);
    //Objects
    IEnumerable<MapObject> GetMapObjects(JurassicParkDbContext context, SavedGame savedGame);
    Task<Result<MapObject, ServiceError>> GetMapObjectById(JurassicParkDbContext context, long id);
    Task<Option<ServiceError>> CreateMapObject(JurassicParkDbContext context, SavedGame savedGame, MapObject mapObject);
    Task<Option<ServiceError>> UpdateMapObject(JurassicParkDbContext context, MapObject mapObject);
    Task<Option<ServiceError>> DeleteMapObject(JurassicParkDbContext context, MapObject mapObject);
}