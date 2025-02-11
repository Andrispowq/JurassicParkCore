// See https://aka.ms/new-console-template for more information

using JurassicParkCore.DataSchemas;
using JurassicParkTester;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

SQLitePCL.Batteries.Init();

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext<JurassicParkDbContext>(options =>
        {
            options.UseSqlite("Data Source=JurassicParkTest.db");
        });
        
        services.AddScoped<TestService, TestService>();
    }).ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();
    
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JurassicParkDbContext>();
    await dbContext.Database.MigrateAsync();
    
    var service = scope.ServiceProvider.GetRequiredService<TestService>();
    service.Run();
}

await host.RunAsync();