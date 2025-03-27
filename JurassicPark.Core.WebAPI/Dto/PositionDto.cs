using JurassicPark.Core.DataSchemas;

namespace JurassicPark.Core.WebAPI.Dto;

public class PositionDto(Position position)
{
    public long Id => position.Id;
    public double X => position.X;
    public double Y => position.Y;
}

public class CreatePositionDto
{
    public required double X { get; init; }
    public required double Y { get; init; }
}