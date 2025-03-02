using JurassicPark.Core.Config;
using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services;
using JurassicPark.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, ICoreConfiguration config)
    {
        return services.AddPooledDbContextFactory<JurassicParkDbContext>(options =>
            options.UseSqlite(config.ConnectionString)
                .EnableSensitiveDataLogging());
    }
    
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IJeepService, JeepService>();
        services.AddScoped<IMapObjectService, MapObjectService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        services.AddScoped<IGameService, GameService>();
        
        services.AddSingleton<IRandomValueProvider, RandomValueProvider>();

        return services;
    }
}