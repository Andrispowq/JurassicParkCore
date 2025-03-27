namespace JurassicPark.Core.OldModel.Dto
{
    public class AnimalGroup
    {
        public long Id { get; set; }
        public Position? NextPointOfInterest { get; set; }
        public AnimalType GroupType { get; set; }
    }
}