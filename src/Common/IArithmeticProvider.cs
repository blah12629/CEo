using CEo.ArithmeticProviders;
using System;
using System.Collections.Concurrent;
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

    public interface IArithmeticProvider<T> : IArithmeticProvider where T : struct
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

    public class ArithmeticProvider<T> : IArithmeticProvider<T> where T : struct
    {
        /// <summary>
        ///   <para>
        ///     Exposes a thread-safe dictionary which contains existing
        ///       <see cref="IArithmeticProvider"/> implementations and where new
        ///       <see cref="IArithmeticProvider" /> implementations can be added to.
        ///   </para>
        /// </summary>
        public static ConcurrentDictionary<Type, IArithmeticProvider> Providers { get; } =
            new ConcurrentDictionary<Type, IArithmeticProvider>
            {
                [typeof(Double)] = new DoubleArithmeticProvider(),
                [typeof(Int32)] = new Int32ArithmeticProvider(),
                [typeof(Single)] = new SingleArithmeticProvider(),
                [typeof(UInt32)] = new UInt32ArithmeticProvider()
            };

        /// <summary>
        ///   <para>
        ///     Initializes a new instance of <see cref="ArithmeticProvider{T}" />.
        ///   </para>
        /// </summary>
        /// <inheritdoc cref="GetProviderIfExists" />
        public ArithmeticProvider() => Provider = GetProviderIfExists();

        protected IArithmeticProvider Provider { get; set; }

        /// <summary>
        ///   <para>
        ///     Gets a <see cref="IArithmeticProvider" />
        ///       instance compatible for <see cref="T" />.
        ///   </para>
        /// </summary>
        /// <exception cref="ArgumentException">
        ///   <para>
        ///     Thrown when a compatible <see cref="IArithmeticProvider" /> for
        ///       <see cref="T" /> could not be found in
        ///       <see cref="Providers" />
        ///   </para>
        /// </exception>
        protected internal virtual IArithmeticProvider GetProviderIfExists()
        {
            if (Providers.TryGetValue(typeof(T), out var provider))
                return provider;

            var keys = Providers.Keys;
            throw new ArgumentException(
                "Generic type `T` must be either" +
                $"{String.Join(", ", keys.SkipLast(1).Select(type => type.Name))} or " +
                keys.Last().Name);
        }

        public virtual T Add(T left, T right) => (T)Provider.Add(left, right);
        public virtual T Subtract(T left, T right) => (T)Provider.Subtract(left, right);
        public virtual T Multiply(T left, T right) => (T)Provider.Multiply(left, right);
        public virtual T Divide(T left, T right) => (T)Provider.Divide(left, right);
        public virtual T Negate(T value) => (T)Provider.Negate(value);
    }
}