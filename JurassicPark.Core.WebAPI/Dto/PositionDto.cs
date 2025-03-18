using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class PositionDto(Position position)
{
    public long Id => position.Id;
    public double X => position.X;
    public double Y => position.Y;
}