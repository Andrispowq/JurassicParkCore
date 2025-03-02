using JurassicPark.Core.Config;

namespace JurassicParkTester;

public class CoreConfiguration : ICoreConfiguration
{
    public string ConnectionString => "Data Source=JurassicParkTest.db";
}