using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Core.Model;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services)
    {
        services.AddDatabase(new CoreConfiguration());
        services.AddCoreServices();

        return services;
    }
}