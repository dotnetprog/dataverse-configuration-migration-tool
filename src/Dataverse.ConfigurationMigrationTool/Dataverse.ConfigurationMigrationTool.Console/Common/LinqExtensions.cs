namespace Dataverse.ConfigurationMigrationTool.Console.Common
{
    public static class LinqExtensions
    {
        public static bool AreEnumerablesEqualIgnoreOrder<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return new HashSet<T>(list1).SetEquals(list2);
        }
        public static IEnumerable<IEnumerable<T>> Batch<T>(
        this IEnumerable<T> source, int size)
        {
            T[] bucket = null;
            var count = 0;

            foreach (var item in source)
            {
                if (bucket == null)
                    bucket = new T[size];

                bucket[count++] = item;

                if (count != size)
                    continue;

                yield return bucket.Select(x => x);

                bucket = null;
                count = 0;
            }

            // Return the last bucket with all remaining elements
            if (bucket != null && count > 0)
            {
                Array.Resize(ref bucket, count);
                yield return bucket;
            }
        }
    }
}
