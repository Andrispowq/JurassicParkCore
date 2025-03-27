using JurassicPark.Core;
using JurassicPark.Core.DataSchemas;
using JurassicParkTester;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

SQLitePCL.Batteries.Init();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddDatabase(new CoreConfiguration());
        services.AddCoreServices();
        
        services.AddScoped<DataAccessMocker, DataAccessMocker>();
    }).ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();
    
using (var scope = host.Services.CreateScope())
{
    var dbFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<JurassicParkDbContext>>();
    await using var dbContext = dbFactory.CreateDbContext();
    await dbContext.Database.MigrateAsync();
    
    var service = scope.ServiceProvider.GetRequiredService<DataAccessMocker>();
    await service.Run();
}

await host.RunAsync();