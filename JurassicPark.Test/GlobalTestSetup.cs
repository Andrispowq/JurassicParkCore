using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services;
using JurassicPark.Core.Services.Interfaces;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JurassicPark.Test;

[SetUpFixture]
public class GlobalTestSetup
{
    public static ServiceProvider? ServiceProvider { get; private set; }
    private static SqliteConnection? SharedConnection { get; set; }

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        SharedConnection = new SqliteConnection("Data Source=:memory:");
        SharedConnection.Open();

        var services = new ServiceCollection();

        services.AddDbContext<JurassicParkDbContext>(options =>
            options.UseSqlite(SharedConnection));

        services.AddScoped<IDbContextFactory<JurassicParkDbContext>>(sp =>
        {
            var options = sp.GetRequiredService<DbContextOptions<JurassicParkDbContext>>();
            return new TestDbContextFactory(options);
        });

        services.AddScoped<IAnimalService, AnimalService>();
        services.AddScoped<IJeepService, JeepService>();
        services.AddScoped<IMapObjectService, MapObjectService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<ITransactionService, TransactionService>();
        
        services.AddScoped<IGameService, GameService>();

        ServiceProvider = services.BuildServiceProvider();

        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JurassicParkDbContext>();
        await dbContext.Database.MigrateAsync();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        SharedConnection?.Dispose();
        ServiceProvider?.Dispose();
    }
}