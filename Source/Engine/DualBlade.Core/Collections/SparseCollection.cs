namespace DualBlade.Core.Collections;

public class SparseCollection<T>(int count)
{
    private readonly List<T> items = new(count);
    private readonly Stack<int> freeIndices = new();
    private readonly HashSet<int> freeSet = new();
    public int Count { get; private set; }

    public T this[int index]
    {
        get => items[index];
        set => items[index] = value;
    }

    public int Add(T item)
    {
        int index;
        if (freeIndices.Count > 0)
        {
            index = freeIndices.Pop();
            freeSet.Remove(index);
            items[index] = item;
        }
        else
        {
            items.Add(item);
            index = items.Count;
        }
        Count++;

        return index;
    }

    public int NextFreeIndex()
    {
        if (freeIndices.Count > 0)
        {
            return freeIndices.Peek();
        }

        return items.Count;
    }

    public void Remove(int index)
    {
        if (freeSet.Contains(index))
            throw new InvalidOperationException("Index is already free");

        items[index] = default!;
        freeIndices.Push(index);
        freeSet.Add(index);
        Count--;
    }

    public IEnumerable<T> Values()
    {
        for (int i = 0, k = 0; k < Count; i++)
        {
            if (freeSet.Contains(i))
                continue;

            yield return items[i];
            k++;
        }
    }
}
