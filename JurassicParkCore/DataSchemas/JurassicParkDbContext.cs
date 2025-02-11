using JurassicParkCore.DataSchemas.DataTable;
using Microsoft.EntityFrameworkCore;

namespace JurassicParkCore.DataSchemas;

public class JurassicParkDbContext : DbContext
{
    public DbSet<Animal> AnimalTable { get; set; }
    public DbSet<AnimalType> AnimalTypeTable { get; set; }
    public DbSet<AnimalGroup> AnimalGroupTable { get; set; }
    public DbSet<Jeep> JeepTable { get; set; }
    public DbSet<MapObject> MapObjectTable { get; set; }
    public DbSet<Position> PositionTable { get; set; }
    public DbSet<Transaction> TransactionTable { get; set; }
    
    public DataTable<Animal> Animals { get; }
    public DataTable<AnimalType> AnimalTypes { get; }
    public DataTable<AnimalGroup> AnimalGroups { get; }
    public DataTable<Jeep> Jeeps { get; }
    public DataTable<MapObject> MapObjects { get; }
    public DataTable<Position> Positions { get; }
    public DataTable<Transaction> Transactions { get; }

    public JurassicParkDbContext(DbContextOptions<JurassicParkDbContext> options)
        : base(options)
    {
        Animals = new(this, AnimalTable!);
        AnimalTypes = new(this, AnimalTypeTable!);
        AnimalGroups = new(this, AnimalGroupTable!);
        Jeeps = new(this, JeepTable!);
        MapObjects = new(this, MapObjectTable!);
        Positions = new(this, PositionTable!);
        Transactions = new(this, TransactionTable!);
    }
}