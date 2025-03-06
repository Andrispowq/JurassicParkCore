using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.Services.Interfaces;

public interface IRandomValueProvider
{
    AnimalSex GetSexFor(AnimalType animalType);
    bool RollDice(double threshold);
}