using System;

namespace JurassicPark.Core.OldModel.Functional
{
    public abstract class Option<T>
    {
        public sealed class None : Option<T> {}

        public sealed class Some : Option<T>
        {
            public T Value { get; }
            
            public Some(T value)
            {
                Value = value;
            }
        }

        private Option()
        {
        }

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

        public bool IsNone => this is None;
        public bool IsSome => this is Some;

        public None AsNone
        {
            get
            {
                if (this is None none)
                {
                    return none;
                }

                throw new InvalidOperationException();
            }
        }

        public Some AsSome
        {
            get
            {
                if (this is Some some)
                {
                    return some;
                }

                throw new InvalidOperationException();
            }
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
}