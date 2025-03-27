using System.Collections.Generic;

namespace JurassicPark.Core.OldModel.Dto
{
    public class MapObject
    {
        public long Id { get; set; }
        public decimal ResourceAmount { get; set; }
        public MapObjectType MapObjectType { get; set; }
        public Position Position { get; set; }
        public List<Animal> DiscoveredBy { get; set; }
    }

    public class MapObjectUpdateRequest
    {
        public decimal ResourceAmount { get; set; }
    }
}