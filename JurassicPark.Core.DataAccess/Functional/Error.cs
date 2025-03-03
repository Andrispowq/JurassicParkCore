namespace JurassicPark.Core.Functional;

public class Error(string message)
{
    public string Message { get; } = message;
}