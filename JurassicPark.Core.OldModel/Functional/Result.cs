using System;
using System.Threading.Tasks;

namespace JurassicPark.Core.OldModel.Functional
{
    public sealed class Result<T, TE>
    {
        private readonly T _result;
        private readonly TE _error;

        public bool HasValue { get; }
        public bool IsError { get; }

        private Result(T result)
        {
            _result = result ?? throw new ArgumentNullException(nameof(result));
            _error = default!;
            HasValue = true;
            IsError = false;
        }

        private Result(TE error)
        {
            _error = error ?? throw new ArgumentNullException(nameof(error));
            _result = default!;
            HasValue = false;
            IsError = true;
        }

        public void Match(Action<T> result, Action<TE> error)
        {
            if (HasValue)
                result(_result!);
            else if (IsError)
                error(_error!);
            else
                throw new InvalidOperationException("Result must contain either a value or an error.");
        }

        public Result<TU, TE> Map<TU>(Func<T, TU> map)
        {
            return HasValue ? new Result<TU, TE>(map(_result!)) : new Result<TU, TE>(_error!);
        }

        public Result<T, TF> MapError<TF>(Func<TE, TF> map)
        {
            return IsError ? new Result<T, TF>(map(_error!)) : new Result<T, TF>(_result!);
        }

        public TR Map<TR>(Func<T, TR> action, Func<TE, TR> errorAction)
        {
            return HasValue ? action(_result!) : errorAction(_error!);
        }

        public Task<TR> MapAsync<TR>(Func<T, Task<TR>> action, Func<TE, Task<TR>> errorAction)
        {
            return HasValue ? action(_result!) : errorAction(_error!);
        }

        public T GetValueOrThrow()
        {
            if (HasValue)
                return _result!;
            throw new InvalidOperationException("No result present.");
        }

        public TE GetErrorOrThrow()
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
            if (!(obj is Result<T, TE> other)) return false;
            
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

        public static implicit operator Result<T, TE>(T result)
        {
            return new Result<T, TE>(result);
        }

        public static implicit operator Result<T, TE>(TE error)
        {
            return new Result<T, TE>(error);
        }

        public static Result<T, TE> Success(T result) => new Result<T, TE>(result);
        public static Result<T, TE> Failure(TE error) => new Result<T, TE>(error);
    }
}