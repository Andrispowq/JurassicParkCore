using System.Collections.Generic;

namespace JurassicPark.Core.OldModel.Dto
{
    public class Animal
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public int Age { get; set; }
        public AnimalSex Sex { get; set; }
        public bool HasChip { get; set; }
        public AnimalState State { get; set; }

        public decimal HungerLevel { get; set; }
        public decimal ThirstLevel { get; set; }
        public decimal Health { get; set; }

        public AnimalType AnimalType { get; set; }
        public Position Position { get; set; }
        public Position? PointOfInterest { get; set; }
        public AnimalGroup? Group { get; set; }

        public List<MapObject> DiscoveredMapObjects { get; set; }
    }

    public class AnimalUpdateRequest
    {
        public int Age { get; set; }
        public bool HasChip { get; set; }
        public AnimalState State { get; set; }

        public decimal HungerLevel { get; set; }
        public decimal ThirstLevel { get; set; }
        public decimal Health { get; set; }

        public CreatePositionDto Position { get; set; }
    }
}