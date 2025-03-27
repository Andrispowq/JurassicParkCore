namespace JurassicPark.Core.OldModel.Dto
{
    public class Jeep
    {
        public long Id { get; set; }
        public int SeatedVisitors { get; set; }
        public decimal RouteProgression { get; set; }
        public JeepRoute? Route { get; set; }
    }
}