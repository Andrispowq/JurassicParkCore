using JurassicPark.Core.Config;

namespace JurassicPark.Test;

public class CoreConfiguration : ICoreConfiguration
{
    public string ConnectionString => "Data Source=:memory:";
}