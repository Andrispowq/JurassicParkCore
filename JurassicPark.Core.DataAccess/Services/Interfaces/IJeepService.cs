using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Functional;

namespace JurassicPark.Core.Services.Interfaces;

public interface IJeepService
{
    //Jeeps
    IEnumerable<Jeep> GetJeeps(JurassicParkDbContext context, SavedGame game);
    Task<Result<Jeep, ServiceError>> GetJeepById(JurassicParkDbContext context, long id);
    Task<Option<ServiceError>> CreateJeep(JurassicParkDbContext context, SavedGame savedGame, Jeep jeep);
    Task<Option<ServiceError>> UpdateJeep(JurassicParkDbContext context, Jeep jeep);
    Task<Option<ServiceError>> DeleteJeep(JurassicParkDbContext context, Jeep jeep);
    
    //Routes
    IEnumerable<JeepRoute> GetRoutes(JurassicParkDbContext context, SavedGame game);
    Task<Result<JeepRoute, ServiceError>> GetRouteById(JurassicParkDbContext context, long id);
    Task<Option<ServiceError>> CreateRoute(JurassicParkDbContext context, SavedGame savedGame, JeepRoute route);
    Task<Option<ServiceError>> UpdateRoute(JurassicParkDbContext context, JeepRoute route);
    Task<Option<ServiceError>> DeleteRoute(JurassicParkDbContext context, JeepRoute route);
    
    //Jeep-Route Interaction
    Task<Option<ServiceError>> StartRoute(JurassicParkDbContext context, JeepRoute route, Jeep jeep);
    Task<Option<ServiceError>> FinishRoute(JurassicParkDbContext context, Jeep jeep);
}