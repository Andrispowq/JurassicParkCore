namespace JurassicPark.Core.OldModel.Dto
{
    public class AnimalType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public AnimalEatingHabit EatingHabit { get; set; }
        public decimal Price { get; set; }
        public decimal VisitorSatisfaction { get; set; }
    }

    public class CreateAnimalTypeRequest
    {
        public string Name { get; set; }
        public AnimalEatingHabit EatingHabit { get; set; }
        public decimal Price { get; set; }
        public decimal VisitorSatisfaction { get; set; }
    }
}