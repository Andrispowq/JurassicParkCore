using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.WebAPI.Dto;

public class JeepDto(Jeep jeep)
{
    public long Id => jeep.Id;
    public int SeatedVisitors => jeep.SeatedVisitors;
    public decimal RouteProgression => jeep.RouteProgression;
    public JeepRouteDto? Route => jeep.Route is null ? null : new JeepRouteDto(jeep.Route);
}

public static class JeepExtensions
{
    public static async Task<JeepDto> ToDtoAsync(this Jeep jeep, IGameService gameService)
    {
        await jeep.LoadNavigationProperties(gameService);
        return new JeepDto(jeep);
    }
}