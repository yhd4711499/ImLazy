using System.Linq;
// ReSharper disable once CheckNamespace


namespace System.Collections.Generic
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static void ForEach(this IEnumerable items, Action<object> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        public static int Sum(this IEnumerable items)
        {
            var sum = 1;
            items.ForEach(_ =>
            {
                sum++;
                var enumerable = _ as IEnumerable;
                if (enumerable != null)
                {
                    sum += Sum(enumerable);
                }
            });
            return sum;
        }

        public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dic, Func<TKey,bool> func)
        {
            var pairsToRemove = new List<KeyValuePair<TKey, TValue>>(dic.Count);
            pairsToRemove.AddRange(dic.Where(item => func(item.Key)));
            while (pairsToRemove.Count!=0)
            {
                dic.Remove(pairsToRemove[0].Key);
                pairsToRemove.RemoveAt(0);
            }
        }
    }
}
