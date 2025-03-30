using JurassicPark.Core.OldModel.Dto;

namespace JurassicPark.Core.OldModel.Services
{
    public interface IRandomValueProvider
    {
        AnimalSex GetSexFor(AnimalType animalType);
        bool RollDice(double threshold);
    }
}