namespace JurassicPark.Core.OldModel.Dto
{
    public class Jeep
    {
        public long Id { get; set; }
        public int SeatedVisitors { get; set; }
        public decimal RouteProgression { get; set; }
        public JeepRoute? Route { get; set; }
    }

    public class UpdateJeepRequest
    {
        public int SeatedVisitors { get; set; }
        public decimal RouteProgression { get; set; }
        public long? RouteId { get; set; }
    }
}