using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;

namespace JurassicParkCore.Services.Interfaces;

public interface IMapObjectService
{
    //Types
    IEnumerable<MapObjectType> GetMapObjectTypes(JurassicParkDbContext context);
    Task<Option<ServiceError>> CreateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType);
    //Objects
    IEnumerable<MapObject> GetMapObjects(JurassicParkDbContext context, SavedGame savedGame);
    Task<Option<ServiceError>> CreateMapObject(JurassicParkDbContext context, SavedGame savedGame, MapObject mapObject);
    Task<Option<ServiceError>> UpdateMapObject(JurassicParkDbContext context, MapObject mapObject);
    Task<Option<ServiceError>> DeleteMapObject(JurassicParkDbContext context, MapObject mapObject);
}