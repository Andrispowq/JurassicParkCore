using JurassicPark.Core.Config;

namespace JurassicPark.Core.WebAPI;

public class CoreConfiguration : ICoreConfiguration
{
    private static ICoreConfiguration? _instance;
    public static ICoreConfiguration Instance
    {
        get
        {
            return _instance ??= new CoreConfiguration();
        }
    }
    
    private CoreConfiguration() {}
    
    public string ConnectionString => "Data Source=JurassicParkTest.db";
}