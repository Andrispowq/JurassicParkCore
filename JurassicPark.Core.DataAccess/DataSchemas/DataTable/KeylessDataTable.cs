using JurassicPark.Core.Functional;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Core.DataSchemas.DataTable;

public interface IKeylessDataTable<in T>
    where T : class
{
    public Task<Option<DatabaseError>> Create(T instance);
    public Task<Option<DatabaseError>> Update(T instance);
    public Task<Option<DatabaseError>> Delete(T instance);
}

public class DiscoveriesDataTable(DbContext context, DbSet<Discovered> dbSet) : IKeylessDataTable<Discovered>
{
    public DbSet<Discovered> All { get; } = dbSet;

    private async Task<Discovered?> FindAsync(Discovered discovered)
    {
        return await All.FindAsync(discovered.AnimalId, discovered.MapObjectId);
    }

    public async Task<Option<DatabaseError>> Create(Discovered instance)
    {
        var existing = await FindAsync(instance);
        if (existing is not null)
        {
            return new EntryAlreadyExistsError();
        }

        All.Add(instance);

        try
        {
            await context.SaveChangesAsync();
            return new Option<DatabaseError>.None();
        }
        catch (Exception e)
        {
            return new DatabaseError(e.Message);
        }
    }
    
    public async Task<Option<DatabaseError>> Update(Discovered instance)
    {
        var existing = await FindAsync(instance);
        if (existing is null)
        {
            return new EntryNotFoundError();
        }

        All.Update(instance);

        try
        {
            await context.SaveChangesAsync();
            return new Option<DatabaseError>.None();
        }
        catch (Exception e)
        {
            return new DatabaseError(e.Message);
        }
    }
    
    public async Task<Option<DatabaseError>> Delete(Discovered instance)
    {
        var existing = await FindAsync(instance);
        if (existing is null)
        {
            return new EntryNotFoundError();
        }
        
        All.Remove(instance);

        try
        {
            await context.SaveChangesAsync();
            return new Option<DatabaseError>.None();
        }
        catch (Exception e)
        {
            return new DatabaseError(e.Message);
        }
    }
}