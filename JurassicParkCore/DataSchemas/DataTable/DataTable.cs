using JurassicParkCore.Functional;
using Microsoft.EntityFrameworkCore;

namespace JurassicParkCore.DataSchemas.DataTable;

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
        await context.SaveChangesAsync();
        return new Option<DatabaseError>.None();
    }
    
    public async Task<Option<DatabaseError>> Update(T instance)
    {
        var existing = await All.FindAsync(instance.Id);
        if (existing is null)
        {
            return new EntryNotFoundError();
        }

        All.Update(instance);
        await context.SaveChangesAsync();
        return new Option<DatabaseError>.None();
    }
    
    public async Task<Option<DatabaseError>> Delete(T instance)
    {
        var existing = await All.FindAsync(instance.Id);
        if (existing is null)
        {
            return new EntryNotFoundError();
        }
        
        All.Remove(instance);
        await context.SaveChangesAsync();
        return new Option<DatabaseError>.None();
    }
}