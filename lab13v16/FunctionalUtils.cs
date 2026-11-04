using System;
using System.Collections.Generic;

namespace lab13v16
{
    public static class FunctionalUtils
    {
     
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> mapper)
        {
            foreach (var item in source)
            {
                yield return mapper(item);
            }
        }

      
        public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

     
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector) where TKey : IComparable<TKey>
        {
            using var enumerator = source.GetEnumerator();
            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Колекція порожня.");

            T maxItem = enumerator.Current;
            TKey maxKey = keySelector(maxItem);

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                var key = keySelector(item);
                if (key.CompareTo(maxKey) > 0)
                {
                    maxKey = key;
                    maxItem = item;
                }
            }

            return maxItem;
        }

        public static double Average<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            double sum = 0;
            int count = 0;
            foreach (var item in source)
            {
                sum += selector(item);
                count++;
            }
            if (count == 0)
                throw new InvalidOperationException("Неможливо порахувати середнє для порожньої колекції.");
            
            return sum / count;
        }

    
        public static bool Any<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
            {
                if (predicate(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
