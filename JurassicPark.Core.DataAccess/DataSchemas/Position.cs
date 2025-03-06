using System.ComponentModel.DataAnnotations;

namespace JurassicPark.Core.DataSchemas;

public record Position : IKeyedDataType
{
    public static readonly double DoubleDelta = 0.0001;

    [Key] public long Id { get; set; }
    public long? JeepRouteId { get; set; }
    public required double X { get; set; }
    public required double Y { get; set; }

    public bool EqualsTo(Position other)
    {
        if (Id == other.Id) return true;
        
        return Math.Abs(X - other.X) < DoubleDelta 
               && Math.Abs(Y - other.Y) < DoubleDelta;
    }

    public bool IsAdjacentTo(Position position)
    {
        var posX = (int)Math.Floor(X);
        var posY = (int)Math.Floor(Y);
        
        var otherPosX = (int)Math.Floor(position.X);
        var otherPosY = (int)Math.Floor(position.Y);
        
        return (posX == otherPosX && Math.Abs(posY - otherPosY) == 1)
            || (posY == otherPosY && Math.Abs(posX - otherPosX) == 1);
    }

    public double DistanceTo(Position other)
    {
        return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
    }
    
    public bool IsRoundedDown => X - (int)Math.Floor(X) < DoubleDelta && Y - (int)Math.Floor(Y) < DoubleDelta;
}