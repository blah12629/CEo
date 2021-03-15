using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Split<T>(
            this IEnumerable<T> collection,
            Int32 splitSize)
        {
            Single collectionSize = collection.Count(),
                maximumI = collectionSize / splitSize;

            for (var i = 0; i < maximumI; i ++)
                yield return collection.Skip(i * splitSize).Take(splitSize);
        }

        public static IEnumerable<T> ElementsAt<T>(
            this IEnumerable<T> collection,
            params Int32[] indices) =>
                collection.ElementsAt(indices as IEnumerable<Int32>);

        public static IEnumerable<T> ElementsAt<T>(
            this IEnumerable<T> collection,
            IEnumerable<Int32> indices)
        {
            foreach (var index in indices)
                yield return collection.ElementAt(index);
        }
    }
}