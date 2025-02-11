using JurassicParkCore.DataSchemas;

namespace JurassicParkTester;

public class TestService(JurassicParkDbContext dbContext)
{
    public void Run()
    {
        var animals = dbContext.AnimalTypes.All;
        foreach (var animal in animals)
        {
            Console.WriteLine(animal);
        }
    }
}