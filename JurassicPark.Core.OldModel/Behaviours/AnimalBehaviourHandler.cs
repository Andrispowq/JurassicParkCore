using System;
using System.Linq;
using System.Threading.Tasks;
using JurassicPark.Core.OldModel.Dto;
using JurassicPark.Core.OldModel.Services;

namespace JurassicPark.Core.OldModel.Behaviours
{
    public class AnimalBehaviourHandler : IAnimalBehaviourHandler
    {
        public decimal HealthIncreasePerMinute => 5;
        public decimal ThirstIncreasePerMinute => 7;
        public decimal DamagePerMinute => 4;
        public decimal FieldOfViewDegrees => 110;
        public decimal ViewDistance => 15; //in in-game tiles
        public decimal AcceptedPointOfInterestRadius => ViewDistance * 2 / 3; //in in-game tiles

        private static decimal ThirstThreshold => 60;
        private static decimal HungerThreshold => 60;
        private static decimal DigestionThreshold => 20;
        private static double MatingChangePercent => 2; //2 percent change every second
        private static double ConsumeDistance => 3;
        
        private readonly IRandomValueProvider _randomValueProvider;

        public AnimalBehaviourHandler(IRandomValueProvider randomValueProvider)
        {
            _randomValueProvider = randomValueProvider;
        }

        public async Task ApplySingleChangesAsync(IJurassicParkModel model, Animal animal, double delta)
        {
            //Apply living things
            var dead = await ApplyLivingEffects(model, animal, delta);
            if (dead) return;

            //Get state
            var state = DetermineState(model, animal, delta);
            animal.State = state; //Not necessary to save but gets saved anyway

            //Handle the state action
            switch (state)
            {
                case AnimalState.Eating:
                    //TODO: get what its eating and eat it
                    break;
                case AnimalState.Drinking:
                    //TODO: get what its drinking and drink it
                    break;
                case AnimalState.SearchingGroup:
                    //TODO: find a group in its FOV or go randomly
                    break;
                case AnimalState.Mating:
                    //TODO: child
                    break;
                case AnimalState.Hungry:
                    //TODO: search for water
                    break;
                case AnimalState.Thirsty:
                    //TODO: search for food
                    break;
                case AnimalState.Digesting:
                    break;
                case AnimalState.HangingOutInGroup:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Task ApplyGroupChangesAsync(IJurassicParkModel model, double delta)
        {
            throw new NotImplementedException();
        }

        private AnimalState DetermineState(IJurassicParkModel model, Animal animal, double delta)
        {
            //Most important thing: thirst/drinking
            if (animal.ThirstLevel > ThirstThreshold)
            {
                var hasNearby = HasWaterSourceNearby(model, animal) != null;
                return hasNearby ? AnimalState.Drinking : AnimalState.Thirsty;
            }

            //Second most important: hunger/eating
            if (animal.HungerLevel > HungerThreshold)
            {
                var hasNearby = animal.AnimalType.EatingHabit switch
                {
                    AnimalEatingHabit.Carnivore => HasCarnivoreFoodSourceNearby(model, animal) != null,
                    _ => HasHerbivoreFoodSourceNearby(model, animal) != null
                };

                return hasNearby ? AnimalState.Eating : AnimalState.Hungry;
            }

            //If the animal has recently eaten/drank, the state is digestion
            if (animal.HungerLevel < DigestionThreshold || animal.ThirstLevel < DigestionThreshold)
            {
                return AnimalState.Digesting;
            }

            //if no group, search one
            if (animal.Group is null) return AnimalState.SearchingGroup;

            //if in a group, either mate or just hang out
            var mating = _randomValueProvider.RollDice(delta * MatingChangePercent);
            return mating ? AnimalState.Mating : AnimalState.HangingOutInGroup;
        }

        private MapObject? HasWaterSourceNearby(IJurassicParkModel model, Animal animal)
        {
            var waterSources = model.MapObjects.Where(m => m.MapObjectType.ResourceType == ResourceType.Water);
            return waterSources.FirstOrDefault(m => m.Position.DistanceTo(animal.Position) < ConsumeDistance);
        }

        private MapObject? HasHerbivoreFoodSourceNearby(IJurassicParkModel model, Animal animal)
        {
            var waterSources = model.MapObjects.Where(m => m.MapObjectType.ResourceType == ResourceType.Vegetation);
            return waterSources.FirstOrDefault(m => m.Position.DistanceTo(animal.Position) < ConsumeDistance);
        }

        private Animal? HasCarnivoreFoodSourceNearby(IJurassicParkModel model, Animal animal)
        {
            var foodSources = model.Animals.Where(m => m.AnimalType.EatingHabit == AnimalEatingHabit.Herbivore);
            return foodSources.FirstOrDefault(m => m.Position.DistanceTo(animal.Position) < ConsumeDistance);
        }

        private async Task<bool> ApplyLivingEffects(IJurassicParkModel model, Animal animal, double delta)
        {
            //Health, hunger and thirst
            animal.ThirstLevel += ThirstIncreasePerMinute * (decimal)delta;
            animal.HungerLevel += HealthIncreasePerMinute * (decimal)delta;

            var applyDamage = false;
            if (animal.ThirstLevel > 100)
            {
                animal.ThirstLevel = 100;
                applyDamage = true;
            }

            if (animal.HungerLevel > 100)
            {
                animal.HungerLevel = 100;
                applyDamage = true;
            }

            if (!applyDamage) return false;
            
            animal.Health -= HealthIncreasePerMinute * (decimal)delta;
            if (animal.Health > 0) return false;
            
            await model.KillAnimal(animal);
            return true; //The animal died
        }
    }
}