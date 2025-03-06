using JurassicPark.Core;
using JurassicPark.Core.DataSchemas;
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
            options.UseSqlite(SharedConnection)
                .EnableSensitiveDataLogging());

        services.AddScoped<IDbContextFactory<JurassicParkDbContext>>(sp =>
        {
            var options = sp.GetRequiredService<DbContextOptions<JurassicParkDbContext>>();
            return new TestDbContextFactory(options);
        });
        
        services.AddCoreServices();

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