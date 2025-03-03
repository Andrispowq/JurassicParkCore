namespace JurassicPark.Core.DataSchemas;

public record Discovered
{
    // Foreign key to Animal
    public required long AnimalId { get; set; }
    
    // Foreign key to MapObject
    public required long MapObjectId { get; set; }

    // Navigation properties
    public virtual Animal Animal { get; set; } = null!;
    public virtual MapObject MapObject { get; set; } = null!;
}