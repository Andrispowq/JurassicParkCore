using JurassicPark.Core.Functional;
using Microsoft.EntityFrameworkCore;

namespace JurassicPark.Core.DataSchemas.DataTable;

public class DataTable<T>(DbContext context, DbSet<T> dbSet) : IDataTable<T>
    where T : class, IKeyedDataType
{
    public DbSet<T> All { get; } = dbSet;

    public async Task<Result<T, DatabaseError>> Get(int id)
    {
        var item = await All.FindAsync(id);
        if (item is null) return new EntryNotFoundError();
        
        return item;
    }
    
    public async Task<Option<DatabaseError>> Create(T instance)
    {
        var existing = await All.FindAsync(instance.Id);
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
    
    public async Task<Option<DatabaseError>> Update(T instance)
    {
        var existing = await All.FindAsync(instance.Id);
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
    
    public async Task<Option<DatabaseError>> Delete(T instance)
    {
        var existing = await All.FindAsync(instance.Id);
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