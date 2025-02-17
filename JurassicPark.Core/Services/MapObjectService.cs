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

    public async Task<Option<ServiceError>> CreateMapObjectType(JurassicParkDbContext context, MapObjectType mapObjectType)
    {
        var result = await context.MapObjectTypes.Create(mapObjectType);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public IEnumerable<MapObject> GetMapObjects(JurassicParkDbContext context, SavedGame savedGame)
    {
        return context.MapObjects.All.Where(m => m.SavedGameId == savedGame.Id);
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

        await context.Entry(mapObject).Reference(a => a.SavedGame).LoadAsync();
        if (mapObject.SavedGame.GameState != GameState.Ongoing)
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