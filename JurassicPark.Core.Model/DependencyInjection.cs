using JurassicPark.Core.Model.Behaviours;
using JurassicPark.Core.Model.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Core.Model;

public static class DependencyInjection
{
    private static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddDatabase(new CoreConfiguration());
        services.AddCoreServices();

        return services;
    }

    public static IServiceCollection AddModel(this IServiceCollection services)
    {
        services.AddDataAccess();

        services.AddScoped<IRouteValidator, RouteValidator>();
        services.AddScoped<IAnimalBehaviourHandler, AnimalBehaviourHandler>();
        
        return services;
    }
}