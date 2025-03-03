using JurassicPark.Core.Config;

namespace JurassicPark.Core.Model;

public class CoreConfiguration : ICoreConfiguration
{
    public string ConnectionString => "Data Source=JurassicParkTest.db";
}