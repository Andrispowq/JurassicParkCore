namespace JurassicPark.Core.DataSchemas;

public interface ITimestamped
{
    public DateTime CreatedAt { get; init; }
}