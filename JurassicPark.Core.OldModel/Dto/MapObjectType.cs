namespace JurassicPark.Core.OldModel.Dto
{
    public class MapObjectType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ResourceType ResourceType { get; set; }
        public decimal ResourceAmount { get; set; }
        public decimal Price { get; set; }
    }

    public class CreateMapObjectTypeDto
    {
        public string Name { get; set; }
        public ResourceType ResourceType { get; set; }
        public decimal ResourceAmount { get; set; }
        public decimal Price { get; set; }
    }
}