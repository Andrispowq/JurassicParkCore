using JurassicPark.Core.DataSchemas;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Test;

public class TestDbContextFactory(DbContextOptions<JurassicParkDbContext> options)
    : IDbContextFactory<JurassicParkDbContext>
{
    public JurassicParkDbContext CreateDbContext()
    {
        return new JurassicParkDbContext(options);
    }
}