using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.WebAPI.Dto;

public class JeepRouteDto(JeepRoute route)
{
    public string Name => route.Name;
    public ICollection<PositionDto> RoutePositions => route.RoutePositions.Select(p => new PositionDto(p)).ToList();
}

public static class JeepRouteExtensions
{
    public static async Task<JeepRouteDto> ToDtoAsync(this JeepRoute route, IGameService gameService)
    {
        await route.LoadNavigationProperties(gameService);
        return new JeepRouteDto(route);
    }
}