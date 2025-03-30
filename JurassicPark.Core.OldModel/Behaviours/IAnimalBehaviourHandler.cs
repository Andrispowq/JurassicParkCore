using JurassicPark.Core.OldModel.Dto;
using System.Threading.Tasks;

namespace JurassicPark.Core.OldModel.Behaviours
{
    public interface IAnimalBehaviourHandler
    {
        decimal HealthIncreasePerMinute { get; }
        decimal ThirstIncreasePerMinute { get; }
        decimal DamagePerMinute { get; }
        decimal FieldOfViewDegrees { get; }
        decimal ViewDistance { get; }
        decimal AcceptedPointOfInterestRadius { get; }

        Task ApplySingleChangesAsync(IJurassicParkModel model, Animal animal, double delta);
        Task ApplyGroupChangesAsync(IJurassicParkModel model, double delta);
    }
}