using CEo.ArithmeticProviders;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CEo
{
    public interface IArithmeticProvider
    {
        Object Add(Object left, Object right);
        Object Subtract(Object left, Object right);
        Object Multiply(Object left, Object right);
        Object Divide(Object left, Object right);
        Object Negate(Object value);
    }

    public interface IArithmeticProvider<T> : IArithmeticProvider where T : notnull
    {
        Object IArithmeticProvider.Add(Object left, Object right) =>
            Add(convert(left), convert(right));
        Object IArithmeticProvider.Subtract(Object left, Object right) =>
            Subtract(convert(left), convert(right));
        Object IArithmeticProvider.Multiply(Object left, Object right) =>
            Multiply(convert(left), convert(right));
        Object IArithmeticProvider.Divide(Object left, Object right) =>
            Divide(convert(left), convert(right));
        Object IArithmeticProvider.Negate(Object value) => Negate(convert(value));
        private T convert(Object value) => (T)Convert.ChangeType(value, typeof(T));

        T Add(T left, T right);
        T Subtract(T left, T right);
        T Multiply(T left, T right);
        T Divide(T left, T right);
        T Negate(T value);
    }

    public class ArithmeticProvider<T> : IArithmeticProvider<T> where T : notnull
    {
        protected static ConcurrentDictionary<Type, IArithmeticProvider> Providers { get; } =
            new ConcurrentDictionary<Type, IArithmeticProvider>(new[]
            {
                KeyValuePair.Create<Type, IArithmeticProvider>(typeof(Double), new DoubleArithmeticProvider()),
                KeyValuePair.Create<Type, IArithmeticProvider>(typeof(Int32), new Int32ArithmeticProvider()),
                KeyValuePair.Create<Type, IArithmeticProvider>(typeof(Single), new SingleArithmeticProvider()),
                KeyValuePair.Create<Type, IArithmeticProvider>(typeof(UInt32), new UInt32ArithmeticProvider()),
            });

        public ArithmeticProvider()
        {
            if (Providers.TryGetValue(typeof(T), out var provider))
                Provider = provider;
            else
            {
                var keys = Providers.Keys;
                throw new ArgumentException(
                    "Generic type `T` must be either" +
                    $"{String.Join(", ", keys.SkipLast(1).Select(type => type.Name))} or " +
                    keys.Last().Name);
            }
        }

        protected IArithmeticProvider Provider { get; }

        public virtual T Add(T left, T right) => (T)Provider.Add(left, right);
        public virtual T Subtract(T left, T right) => (T)Provider.Subtract(left, right);
        public virtual T Multiply(T left, T right) => (T)Provider.Multiply(left, right);
        public virtual T Divide(T left, T right) => (T)Provider.Divide(left, right);
        public virtual T Negate(T value) => (T)Provider.Negate(value);
    }
}