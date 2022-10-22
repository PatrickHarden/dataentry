using System;
using System.Collections.Generic;
using System.Linq;

namespace dataentry.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// (Polyfill for method introduced in .Net 6) Returns distinct elements from a sequence according to a specified key selector function.
        /// </summary>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector
        ) => source.GroupBy(keySelector).Select(g => g.First());

        /// <summary>
        /// Returns the most common value specified by <paramref name="keySelector" />
        /// </summary>
        /// <param name="source">The sequence to find the modal key in.</param>
        /// <param name="keySelector">A function to extract the key for each element.</param>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <typeparam name="TKey">The type of key to distinguish elements by.</typeparam>
        public static TKey Mode<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector,
            TKey defaultValue = default
        ) where TKey : class
        {
            if (source == null)
                return defaultValue;
            var counts = new Dictionary<TKey, int>();
            foreach (var value in source)
            {
                var key = keySelector(value);
                if (!counts.TryGetValue(key, out int count))
                    count = 0;
                counts[key] = count + 1;
            }
            if (counts.Count == 0)
                return defaultValue;
            return counts.Aggregate((key1, key2) => key1.Value >= key2.Value ? key1 : key2).Key;
        }

        /// <summary>
        /// Returns the first element in a sequence that <paramref name="condition"/> evaluates as true.
        /// If there are no matching elements, returns the first element of the sequence.
        /// If there are no elements in the sequence, returns the default of <typeparamref name="T"/>
        /// /// </summary>
        /// <param name="source">The sequence to retrieve a value from.</param>
        /// <param name="condition">The preferred condition.</param>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <returns></returns>
        public static T Prefer<T>(this IEnumerable<T> source, Func<T, bool> condition) =>
            source.Where(condition).Concat(source).FirstOrDefault();

        public static TSource WithMax<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable =>
            source != null && source.Any()
                ? source.Aggregate((v1, v2) => selector(v1).CompareTo(selector(v2)) >= 0 ? v1 : v2)
                : default;
                
        public static TSource WithMin<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable =>
            source != null && source.Any()
                ? source.Aggregate((v1, v2) => selector(v1).CompareTo(selector(v2)) <= 0 ? v1 : v2)
                : default;
    }
}
