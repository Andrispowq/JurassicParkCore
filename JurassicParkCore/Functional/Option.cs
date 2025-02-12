namespace JurassicParkCore.Functional;

public abstract record Option<T>
{
    public sealed record None : Option<T>;

    public sealed record Some(T Value) : Option<T>;
    
    private Option() { }

    public TOut Map<TOut>(Func<T, TOut> someFunc, Func<TOut> noneFunc)
    {
        if (this is Some some)
        {
            return someFunc(some.Value);
        }
        
        return noneFunc();
    }
    
    public Option<TOut> MapOption<TOut>(Func<T, Option<TOut>> someFunc)
    {
        if (this is Some some)
        {
            return someFunc(some.Value);
        }
        
        return new Option<TOut>.None();
    }

    public void ThrowIfNone()
    {
        if (this is None) throw new InvalidOperationException();
    }

    public void ThrowIfSome()
    {
        if (this is Some) throw new InvalidOperationException();
    }

    public static implicit operator Option<T>(T value)
    {
        return new Some(value);
    }
}