using JurassicParkCore.DataSchemas.DataTable;
using Microsoft.EntityFrameworkCore;

namespace JurassicParkCore.DataSchemas;

public class JurassicParkDbContext : DbContext
{
    public DbSet<Animal> AnimalTable { get; set; }
    public DbSet<AnimalType> AnimalTypeTable { get; set; }
    public DbSet<AnimalGroup> AnimalGroupTable { get; set; }
    public DbSet<Jeep> JeepTable { get; set; }
    public DbSet<JeepRoute> JeepRouteTable { get; set; }
    public DbSet<MapObject> MapObjectTable { get; set; }
    public DbSet<MapObjectType> MapObjectTypeTable { get; set; }
    public DbSet<Position> PositionTable { get; set; }
    public DbSet<SavedGame> SavedGameTable { get; set; }
    public DbSet<Transaction> TransactionTable { get; set; }
    
    public DataTable<Animal> Animals { get; }
    public DataTable<AnimalType> AnimalTypes { get; }
    public DataTable<AnimalGroup> AnimalGroups { get; }
    public DataTable<Jeep> Jeeps { get; }
    public DataTable<JeepRoute> JeepRoutes { get; }
    public DataTable<MapObject> MapObjects { get; }
    public DataTable<MapObjectType> MapObjectTypes { get; }
    public DataTable<Position> Positions { get; }
    public DataTable<SavedGame> SavedGames { get; }
    public DataTable<Transaction> Transactions { get; }

    public JurassicParkDbContext(DbContextOptions<JurassicParkDbContext> options)
        : base(options)
    {
        Animals = new(this, AnimalTable!);
        AnimalTypes = new(this, AnimalTypeTable!);
        AnimalGroups = new(this, AnimalGroupTable!);
        Jeeps = new(this, JeepTable!);
        JeepRoutes = new(this, JeepRouteTable!);
        MapObjects = new(this, MapObjectTable!);
        MapObjectTypes = new(this, MapObjectTypeTable!);
        Positions = new(this, PositionTable!);
        SavedGames = new(this, SavedGameTable!);
        Transactions = new(this, TransactionTable!);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AnimalType>()
            .HasIndex(type => type.Name)
            .IsUnique();
        
        modelBuilder.Entity<MapObjectType>()
            .HasIndex(type => type.Name)
            .IsUnique();
        
        modelBuilder.Entity<SavedGame>()
            .HasIndex(type => type.Name)
            .IsUnique();
    }
}