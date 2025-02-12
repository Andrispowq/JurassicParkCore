// See https://aka.ms/new-console-template for more information

using JurassicParkCore.DataSchemas;
using JurassicParkCore.Services;
using JurassicParkCore.Services.Interfaces;
using JurassicParkTester;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

SQLitePCL.Batteries.Init();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddPooledDbContextFactory<JurassicParkDbContext>(options =>
        {
            options.UseSqlite("Data Source=JurassicParkTest.db");
        });
        
        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IJeepService, JeepService>();
        services.AddScoped<IMapObjectService, MapObjectService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<GameMocker, GameMocker>();
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
    
    var service = scope.ServiceProvider.GetRequiredService<GameMocker>();
    await service.MockGame();
}

await host.RunAsync();