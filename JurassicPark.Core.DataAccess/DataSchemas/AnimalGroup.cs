using System.ComponentModel.DataAnnotations;
using JurassicPark.Core.Services.Interfaces;

namespace JurassicPark.Core.DataSchemas;

public record AnimalGroup : IKeyedDataType
{
    [Key] public long Id { get; set; }
    public required long SavedGameId { get; set; }
    public required long? NextPointOfInterestId { get; set; }
    public required long GroupTypeId { get; set; }
    public virtual SavedGame SavedGame { get; set; }  = null!;
    public virtual Position? NextPointOfInterest { get; set; }
    public virtual AnimalType GroupType { get; set; }  = null!;
    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();
    
    public async Task LoadNavigationProperties(IGameService service)
    { 
        if (NextPointOfInterestId != null) await service.LoadReference(this, o => o.NextPointOfInterest);
    }
}