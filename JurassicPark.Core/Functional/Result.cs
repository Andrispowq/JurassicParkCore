namespace JurassicPark.Core.Functional;

public sealed class Result<T, E>
{
    private readonly T _result;
    private readonly E _error;

    public bool HasValue { get; private init; }
    public bool IsError { get; private init; }

    public Result(T result)
    {
        _result = result ?? throw new ArgumentNullException(nameof(result));
        _error = default!;
        HasValue = true;
        IsError = false;
    }

    public Result(E error)
    {
        _error = error ?? throw new ArgumentNullException(nameof(error));
        _result = default!;
        HasValue = false;
        IsError = true;
    }

    public void Match(Action<T> result, Action<E> error)
    {
        if (HasValue)
            result(_result!);
        else if (IsError)
            error(_error!);
        else
            throw new InvalidOperationException("Result must contain either a value or an error.");
    }

    public Result<U, E> Map<U>(Func<T, U> map)
    {
        if (HasValue)
            return new Result<U, E>(map(_result!));
        return new Result<U, E>(_error!);
    }

    public Result<T, F> MapError<F>(Func<E, F> map)
    {
        if (IsError)
            return new Result<T, F>(map(_error!));
        return new Result<T, F>(_result!);
    }

    public R Map<R>(Func<T, R> action, Func<E, R> errorAction)
    {
        if (HasValue)
            return action(_result!);
        return errorAction(_error!);
    }

    public Task<R> MapAsync<R>(Func<T, Task<R>> action, Func<E, Task<R>> errorAction)
    {
        if (HasValue)
            return action(_result!);
        return errorAction(_error!);
    }

    public T GetValueOrThrow()
    {
        if (HasValue)
            return _result!;
        throw new InvalidOperationException("No result present.");
    }

    public E GetErrorOrThrow()
    {
        if (IsError)
            return _error!;
        throw new InvalidOperationException("No error present.");
    }

    public override string ToString()
    {
        if (HasValue)
            return $"Success: {_result}";
        if (IsError)
            return $"Error: {_error}";
        return "Invalid Result";
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Result<T, E> other)
            return false;

        if (HasValue && other.HasValue)
            return Equals(_result, other._result);

        if (IsError && other.IsError)
            return Equals(_error, other._error);

        return false;
    }

    public override int GetHashCode()
    {
        if (HasValue)
            return _result!.GetHashCode();

        if (IsError)
            return _error!.GetHashCode();

        return 0;
    }

    public static implicit operator Result<T, E>(T result)
    {
        return new Result<T, E>(result);
    }

    public static implicit operator Result<T, E>(E error)
    {
        return new Result<T, E>(error);
    }

    public static Result<T, E> Success(T result) => new Result<T, E>(result);
    public static Result<T, E> Failure(E error) => new Result<T, E>(error);
}

public static class ResultExtensions
{
    public static Result<T, E> AsResult<T, E>(this T value)
    {
        return Result<T, E>.Success(value);
    }

    public static Result<T, E> AsError<T, E>(this E error)
    {
        return Result<T, E>.Failure(error);
    }
}