using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public interface IKeyedDataType
{
    public long Id { get; }

    Task LoadNavigationProperties(IGameService service);
}