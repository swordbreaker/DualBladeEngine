namespace DualBlade.Core.Utils;
public class GcCollectionCounter(int[] initialCounts)
{
    public static GcCollectionCounter Start()
    {
        int[] initialCounts = new int[GC.MaxGeneration + 1];

        for (int i = 0; i <= GC.MaxGeneration; i++)
        {
            initialCounts[i] = GC.CollectionCount(i);
        }

        return new GcCollectionCounter(initialCounts);
    }

    public IEnumerable<int> GcCollectAndCountGenerationCollections()
    {
        GC.Collect();

        // Check for changes
        for (int i = 0; i <= GC.MaxGeneration; i++)
        {
            int currentCount = GC.CollectionCount(i);
            yield return currentCount - initialCounts[i];
        }
    }
}
