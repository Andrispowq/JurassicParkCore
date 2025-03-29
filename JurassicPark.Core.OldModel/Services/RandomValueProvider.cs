using System.Collections.Generic;
using System.Security.Cryptography;
using JurassicPark.Core.OldModel.Dto;

namespace JurassicPark.Core.OldModel.Services
{
    public class RandomValueProvider : IRandomValueProvider
    {
        private readonly Dictionary<AnimalType, AnimalSex> _previousSexGenerated = new Dictionary<AnimalType, AnimalSex>();

        public AnimalSex GetSexFor(AnimalType animalType)
        {
            if (_previousSexGenerated.TryGetValue(animalType, out var value))
            {
                var newValue = value switch
                {
                    AnimalSex.Male => AnimalSex.Female,
                    _ => AnimalSex.Male,
                };

                _previousSexGenerated[animalType] = newValue;
                return newValue;
            }

            var def = AnimalSex.Male;
            _previousSexGenerated[animalType] = def;
            return def;
        }

        public bool RollDice(double threshold)
        {
            var number = RandomNumberGenerator.GetInt32(0, int.MaxValue);
            var percent = (double)number / int.MaxValue;
            return percent >= threshold;
        }
    }
}