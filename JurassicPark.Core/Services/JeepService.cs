using JurassicParkCore.DataSchemas;
using JurassicParkCore.Functional;
using JurassicParkCore.Services.Interfaces;

namespace JurassicParkCore.Services;

public class JeepService : IJeepService
{
    public IEnumerable<Jeep> GetJeeps(JurassicParkDbContext context, SavedGame game)
    {
        return context.Jeeps.All.Where(j => j.SavedGameId == game.Id);
    }

    public async Task<Option<ServiceError>> CreateJeep(JurassicParkDbContext context, SavedGame savedGame, Jeep jeep)
    {
        if (savedGame.Id != jeep.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        await context.Entry(jeep).Reference(a => a.SavedGame).LoadAsync();
        if (jeep.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.Jeeps.Create(jeep);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateJeep(JurassicParkDbContext context, Jeep jeep)
    {
        await context.Entry(jeep).Reference(a => a.SavedGame).LoadAsync();
        if (jeep.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.Jeeps.Update(jeep);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteJeep(JurassicParkDbContext context, Jeep jeep)
    {
        await context.Entry(jeep).Reference(a => a.SavedGame).LoadAsync();
        if (jeep.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.Jeeps.Delete(jeep);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public IEnumerable<JeepRoute> GetRoutes(JurassicParkDbContext context, SavedGame game)
    {
        return context.JeepRoutes.All.Where(j => j.SavedGameId == game.Id);
    }

    public async Task<Option<ServiceError>> CreateRoute(JurassicParkDbContext context, SavedGame savedGame, JeepRoute route)
    {
        if (savedGame.Id != route.SavedGameId)
        {
            return new UnauthorizedError("Mismatch in games");
        }
        if (savedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }
        
        await context.Entry(route).Reference(a => a.SavedGame).LoadAsync();
        if (route.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.JeepRoutes.Create(route);
        return result.MapOption<ServiceError>(error => new ConflictError(error.Message));
    }

    public async Task<Option<ServiceError>> UpdateRoute(JurassicParkDbContext context, JeepRoute route)
    {
        await context.Entry(route).Reference(a => a.SavedGame).LoadAsync();
        if (route.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.JeepRoutes.Update(route);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> DeleteRoute(JurassicParkDbContext context, JeepRoute route)
    {
        await context.Entry(route).Reference(a => a.SavedGame).LoadAsync();
        if (route.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        var result = await context.JeepRoutes.Delete(route);
        return result.MapOption<ServiceError>(error => new NotFoundError(error.Message));
    }

    public async Task<Option<ServiceError>> StartRoute(JurassicParkDbContext context, JeepRoute route, Jeep jeep)
    {
        await context.Entry(route).Reference(a => a.SavedGame).LoadAsync();
        if (route.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        if (jeep.RouteId is not null)
        {
            return new ConflictError($"Jeep is already running.");
        }
        
        jeep.RouteId = route.Id;
        return await UpdateJeep(context, jeep);
    }

    public async Task<Option<ServiceError>> FinishRoute(JurassicParkDbContext context, Jeep jeep)
    {
        await context.Entry(jeep).Reference(a => a.SavedGame).LoadAsync();
        if (jeep.SavedGame.GameState != GameState.Ongoing)
        {
            return new UnauthorizedError("Game is over");
        }

        if (jeep.RouteId is null)
        {
            return new ConflictError($"Jeep is not running.");
        }
        
        jeep.RouteId = null;
        return await UpdateJeep(context, jeep);
    }
}