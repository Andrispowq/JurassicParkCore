namespace JurassicPark.Core.OldModel.Dto
{
    public class Position
    {
        public long Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class CreatePositionDto
    {
        public double X { get; set; }
        public double Y { get; set; }
    }
}