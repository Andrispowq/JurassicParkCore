using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Services;

public class MapObjectService : IMapObjectService
{
    public IEnumerable<MapObjectType> GetMapObjectTypes(JurassicParkDbContext context)
    {
        return context.MapObjectTypes.All;
    }

    public async Task<Result<MapObjectType, ServiceError>> GetMapObjectTypeById(JurassicParkDbContext context, long id)
    {
        var type = await context.MapObjectTypes.Get(id);
        return type.Map<Result<MapObjectType, ServiceError>>(t => t, e => new NotFoundError(e.Message));
    }

    public async Task<Option<ServiceError>> CreateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType)
    {
        var result = await context.MapObjectTypes.Create(mapObjectType);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType)
    {
        var result = await context.MapObjectTypes.Update(mapObjectType);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType)
    {
        var result = await context.MapObjectTypes.Delete(mapObjectType);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public IEnumerable<MapObject> GetMapObjects(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.MapObjects.All.Where(m => m.SavedGameId == savedGame.Id);
    }

    public async Task<Result<MapObject, ServiceError>> GetMapObjectById(JurassicParkDbContext context, long id)
    {
        var type = await context.MapObjects.Get(id);
        return type.Map<Result<MapObject, ServiceError>>(t => t, e => new NotFoundError(e.Message));
    }

    public async Task<Option<ServiceError>> CreateMapObject(JurassicParkDbContext context, SavedGame savedGame, MapObject mapObject)
    {
        if (savedGame.Id != mapObject.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.MapObjects.Create(mapObject);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateMapObject(JurassicParkDbContext context, MapObject mapObject)
    {
        await context.Entry(mapObject).Reference(a => a.SavedGame).LoadAsync();
        if (mapObject.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.MapObjects.Update(mapObject);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteMapObject(JurassicParkDbContext context, MapObject mapObject)
    {
        await context.Entry(mapObject).Reference(a => a.SavedGame).LoadAsync();
        if (mapObject.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.MapObjects.Delete(mapObject);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }
}