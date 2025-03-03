using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JurassicPark.Core.DataSchemas;

public class JurassicParkDbContextFactory : IDesignTimeDbContextFactory<JurassicParkDbContext>
{
    public JurassicParkDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JurassicParkDbContext>();
        optionsBuilder.UseSqlite("Data Source=JurassicParkTest.db");

        return new JurassicParkDbContext(optionsBuilder.Options);
    }
}