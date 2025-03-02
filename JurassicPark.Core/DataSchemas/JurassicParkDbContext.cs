using JurassicPark.Core.DataSchemas.DataTable;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Core.DataSchemas;

public sealed class JurassicParkDbContext : DbContext
{
    public DbSet<Animal> AnimalTable { get; set; }
    public DbSet<AnimalType> AnimalTypeTable { get; set; }
    public DbSet<AnimalGroup> AnimalGroupTable { get; set; }
    public DbSet<Discovered> DiscoveredTable { get; set; }
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
    public DiscoveriesDataTable Discoveries { get; }
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
        Discoveries = new(this, DiscoveredTable!);
        Jeeps = new(this, JeepTable!);
        JeepRoutes = new(this, JeepRouteTable!);
        MapObjects = new(this, MapObjectTable!);
        MapObjectTypes = new(this, MapObjectTypeTable!);
        Positions = new(this, PositionTable!);
        SavedGames = new(this, SavedGameTable!);
        Transactions = new(this, TransactionTable!);

        ChangeTracker.LazyLoadingEnabled = false;
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
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
        
        modelBuilder.Entity<Discovered>()
            .HasKey(d => new { d.AnimalId, d.MapObjectId });

        modelBuilder.Entity<Discovered>()
            .HasOne(d => d.Animal)
            .WithMany(a => a.DiscoveredMapObjects)
            .HasForeignKey(d => d.AnimalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Discovered>()
            .HasOne(d => d.MapObject)
            .WithMany(m => m.DiscoveredByAnimals)
            .HasForeignKey(d => d.MapObjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}