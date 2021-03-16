// using System;
// using System.Text.Json.Serialization;

// namespace CEo
// {
//     /// <summary>
//     ///   <para>
//     ///     A record which will be compared using only two properties.  
//     ///   </para>
//     /// </summary>
//     /// <remarks>
//     ///   <para>
//     ///     <see cref="ItemPairRecord{TRecord, TValue}.Item1" /> and
//     ///       <see cref="ItemPairRecord{TRecord, TValue}.Item2" />
//     ///       are only visible to the inheriting records.
//     ///   </para>
//     /// </remarks>
//     /// <typeparam name="TRecord">
//     ///   <para>
//     ///     The inheriting record.
//     ///   </para>
//     /// </typeparam>
//     /// <typeparam name="TValue">
//     ///   <para>
//     ///     The type of the properties to compare.
//     ///   </para>
//     /// </typeparam>
//     public abstract record ItemPairRecord<TRecord, TValue> :
//         IEquatable<TRecord>, IComparable<TRecord>
//         where TRecord : ItemPairRecord<TRecord, TValue>
//         where TValue : IEquatable<TValue>, IComparable<TValue>
//     {
//         public ItemPairRecord() : this(default, default) { }
//         public ItemPairRecord(TValue? item1, TValue? item2) =>
//             (Item1, Item2) = (item1, item2);

//         [JsonIgnore]
//         protected TValue? Item1 { get; init; }
//         [JsonIgnore]
//         protected TValue? Item2 { get; init; }

//         public virtual Boolean Equals(TRecord? other) =>
//             (other?.Equals(default) ?? true) ?
//                 false : ValuesEqual(Item1, other.Item1) && ValuesEqual(Item2, other.Item2);

//         protected virtual Boolean ValuesEqual(TValue? thisValue, TValue? otherValue)
//         {
//             if(thisValue?.Equals(default) ?? true)
//                 return otherValue?.Equals(default) ?? true;

//             return (otherValue?.Equals(default) ?? true) ?
//                 false : thisValue.Equals(otherValue);
//         }
        
//         public virtual Int32 CompareTo(TRecord? other)
//         {
//             if(other?.Equals(default) ?? true) return 1;
//             var comparison1 = CompareValues(Item1, other.Item1);
//             return comparison1 == 0 ? CompareValues(Item2, other.Item2) : comparison1;
//         }

//         protected virtual Int32 CompareValues(TValue? thisValue, TValue? otherValue)
//         {
//             if(thisValue?.Equals(default) ?? true)
//                 return (otherValue?.Equals(default) ?? true) ? 0 : -1;

//             return (otherValue?.Equals(default) ?? true) ?
//                 1 : thisValue.CompareTo(otherValue);
//         }
//     }
// }