using System.Collections.Generic;

namespace JurassicPark.Core.OldModel.Dto
{
    public class JeepRoute
    {
        public string Name { get; set; }
        public ICollection<Position> RoutePositions { get; set; }
    }
}