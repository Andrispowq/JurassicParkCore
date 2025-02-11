namespace JurassicParkCore.Functional;

public sealed class Option<T>
{
    private readonly T? _value;

    private Option(T? value)
    {
        _value = value;
    }

    public bool IsSome => _value != null;
    public bool IsNone => _value == null;

    public static Option<T> Some(T value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null.");
        return new Option<T>(value);
    }

    public static Option<T> None() => new Option<T>(default);

    public void Match(Action<T> some, Action none)
    {
        if (IsSome)
            some(_value!);
        else
            none();
    }

    public U Match<U>(Func<T, U> some, Func<U> none)
    {
        return IsSome ? some(_value!) : none();
    }

    public Task<U> MatchAsync<U>(Func<T, Task<U>> some, Func<Task<U>> none)
    {
        return IsSome ? some(_value!) : none();
    }

    public T GetValueOrThrow()
    {
        if (IsSome)
            return _value!;
        throw new InvalidOperationException("Option has no value.");
    }

    public T GetValueOrDefault(T defaultValue)
    {
        return IsSome ? _value! : defaultValue;
    }

    public Option<U> Map<U>(Func<T, U> map)
        where U : class
    {
        if (IsSome)
            return Option<U>.Some(map(_value!));
        return Option<U>.None();
    }

    public Option<U> FlatMap<U>(Func<T, Option<U>> map)
        where U : class
    {
        if (IsSome)
            return map(_value!);
        return Option<U>.None();
    }

    public override string ToString()
    {
        return IsSome ? $"Some({_value})" : "None";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Option<T> other)
            return Equals(_value, other._value);
        return false;
    }

    public override int GetHashCode()
    {
        return _value?.GetHashCode() ?? 0;
    }

    public static implicit operator Option<T>(T value)
    {
        return new Option<T>(value);
    }
}