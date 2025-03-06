using JurassicPark.Core.DataSchemas;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.Model.Behaviours;

public class AnimalBehaviourHandler(IRandomValueProvider randomValueProvider) : IAnimalBehaviourHandler
{
    public decimal HealthIncreasePerMinute => 5;
    public decimal ThirstIncreasePerMinute => 7;
    public decimal DamagePerMinute => 4;
    public decimal FieldOfViewDegrees => 110;
    public decimal ViewDistance => 15; //in in-game tiles
    public decimal AcceptedPointOfInterestRadius => ViewDistance * 2 / 3; //in in-game tiles
    
    private decimal ThirstThreshold => 60;
    private decimal HungerThreshold => 60;
    private decimal DigestionThreshold => 20;
    private double MatingChangePercent => 2; //2 percent change every second
    private double ConsumeDistance => 3;

    public async Task ApplySingleChangesAsync(IJurassicParkModel model, Animal animal, double delta)
    {
        //Unplaced animals shouldn't be updated
        if (animal.PositionId == 0) return;

        //Apply living things
        var dead = await ApplyLivingEffects(model, animal, delta);
        if (dead) return;
        
        //Get state
        var state = await DetermineState(model, animal, delta);
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
        }
    }

    public Task ApplyGroupChangesAsync(IJurassicParkModel model, double delta)
    {
        throw new NotImplementedException();
    }
    
    private async Task<AnimalState> DetermineState(IJurassicParkModel model, Animal animal, double delta)
    {
        if (animal.ThirstLevel > ThirstThreshold)
        {
            var hasNearby = await HasWaterSourceNearby(model, animal) != null;
            return hasNearby ? AnimalState.Drinking : AnimalState.Thirsty;
        }

        if (animal.HungerLevel > HungerThreshold)
        {
            await model.GameService.LoadReference(animal, a => a.AnimalType);
            var hasNearby = animal.AnimalType.EatingHabit switch
            {
                AnimalEatingHabit.Carnivore => await HasCarnivoreFoodSourceNearby(model, animal) != null,
                _ => await HasHerbivoreFoodSourceNearby(model, animal) != null
            };
            
            return hasNearby ? AnimalState.Eating : AnimalState.Hungry;
        }

        if (animal.HungerLevel < DigestionThreshold || animal.ThirstLevel < ThirstThreshold)
        {
            return AnimalState.Digesting;
        }

        if (animal.GroupId == null) return AnimalState.SearchingGroup;
        
        //Now let's handle group dynamics
        await model.GameService.LoadReference(animal, a => a.Group);
        await model.GameService.LoadCollection(animal.Group!, a => a.Animals);

        var mating = randomValueProvider.RollDice(delta * MatingChangePercent);
        return mating ? AnimalState.Mating : AnimalState.HangingOutInGroup;
    }

    private async Task<MapObject?> HasWaterSourceNearby(IJurassicParkModel model, Animal animal)
    {
        foreach (var mo in model.MapObjects)
        {
            await model.GameService.LoadReference(mo, m => m.MapObjectType);
            await model.GameService.LoadReference(mo, m => m.Position);
        }

        await model.GameService.LoadReference(animal, a => a.Position);
        
        var waterSources = model.MapObjects.Where(m => m.MapObjectType.ResourceType == ResourceType.Water);
        return waterSources.FirstOrDefault(m => m.Position.DistanceTo(animal.Position!) < ConsumeDistance);
    }

    private async Task<MapObject?> HasHerbivoreFoodSourceNearby(IJurassicParkModel model, Animal animal)
    {
        foreach (var mo in model.MapObjects)
        {
            await model.GameService.LoadReference(mo, m => m.MapObjectType);
            await model.GameService.LoadReference(mo, m => m.Position);
        }

        await model.GameService.LoadReference(animal, a => a.Position);
        
        var waterSources = model.MapObjects.Where(m => m.MapObjectType.ResourceType == ResourceType.Vegetation);
        return waterSources.FirstOrDefault(m => m.Position.DistanceTo(animal.Position!) < ConsumeDistance);
    }

    private async Task<Animal?> HasCarnivoreFoodSourceNearby(IJurassicParkModel model, Animal animal)
    {
        foreach (var mo in model.Animals)
        {
            if (mo.PositionId == null) continue;
            await model.GameService.LoadReference(mo, m => m.AnimalType);
            await model.GameService.LoadReference(mo, m => m.Position);
        }
        
        var foodSources = model.Animals.Where(m => m.AnimalType.EatingHabit == AnimalEatingHabit.Herbivore);
        return foodSources.Where(a => a.PositionId != null)
            .FirstOrDefault(m => m.Position!.DistanceTo(animal.Position!) < ConsumeDistance);
    }

    private async Task<bool> ApplyLivingEffects(IJurassicParkModel model, Animal animal, double delta)
    {
        //Health, hunger and thirst
        animal.ThirstLevel += ThirstIncreasePerMinute * (decimal)delta;
        animal.HungerLevel += HealthIncreasePerMinute * (decimal)delta;

        bool applyDamage = false;
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

        if (applyDamage)
        {
            animal.Health -= HealthIncreasePerMinute * (decimal)delta;
            
            if (animal.Health <= 0)
            {
                await model.KillAnimal(animal);
                return true; //The animal died
            }
        }

        return false;
    }
}