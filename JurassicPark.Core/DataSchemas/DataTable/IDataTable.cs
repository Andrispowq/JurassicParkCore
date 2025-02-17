using JurassicPark.Core.Functional;

namespace JurassicPark.Core.DataSchemas.DataTable;

public interface IDataTable<T>
    where T : class, IKeyedDataType
{
    public Task<Result<T, DatabaseError>> Get(int id);
    public Task<Option<DatabaseError>> Create(T instance);
    public Task<Option<DatabaseError>> Update(T instance);
    public Task<Option<DatabaseError>> Delete(T instance);
}