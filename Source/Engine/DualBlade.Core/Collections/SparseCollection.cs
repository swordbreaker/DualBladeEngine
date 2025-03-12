namespace DualBlade.Core.Collections;

/// <summary>
/// Represents a sparse collection that efficiently manages a set of elements with non-contiguous indices.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public class SparseCollection<T>
{
    private readonly List<T> _items;
    private readonly Stack<int> _freeIndices;
    private readonly HashSet<int> _freeSet;

    /// <summary>
    /// Gets the number of active elements contained in the collection.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SparseCollection{T}"/> class with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the collection.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the capacity is less than 0.</exception>
    public SparseCollection(int capacity)
    {
        if (capacity < 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be non-negative.");

        _items = new List<T>(capacity);
        _freeIndices = new Stack<int>();
        _freeSet = new HashSet<int>();
        Count = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SparseCollection{T}"/> class with default capacity.
    /// </summary>
    public SparseCollection() : this(0)
    {
    }

    /// <summary>
    /// Gets or sets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is outside the bounds of the collection.</exception>
    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _items.Count || _freeSet.Contains(index))
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range or points to a free slot.");

            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _items.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

            _items[index] = value;
        }
    }

    /// <summary>
    /// Adds an item to the collection.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The index at which the item was added.</returns>
    public int Add(T item)
    {
        int index;
        if (_freeIndices.Count > 0)
        {
            index = _freeIndices.Pop();
            _freeSet.Remove(index);
            _items[index] = item;
        }
        else
        {
            index = _items.Count;
            _items.Add(item);
        }
        Count++;

        return index;
    }

    /// <summary>
    /// Returns the next index that would be used when adding a new item.
    /// </summary>
    /// <returns>The next available index.</returns>
    public int NextFreeIndex()
    {
        if (_freeIndices.Count > 0)
        {
            return _freeIndices.Peek();
        }

        return _items.Count;
    }

    /// <summary>
    /// Removes the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the index is outside the bounds of the collection.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the index is already marked as free.</exception>
    public void Remove(int index)
    {
        if (index < 0 || index >= _items.Count)
            throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");

        if (_freeSet.Contains(index))
            throw new InvalidOperationException("Index is already free.");

        _items[index] = default!;
        _freeIndices.Push(index);
        _freeSet.Add(index);
        Count--;
    }

    /// <summary>
    /// Returns an enumerable collection of all active values in the collection.
    /// </summary>
    /// <returns>An enumerable of all active values.</returns>
    public IEnumerable<T> Values()
    {
        for (int i = 0, k = 0; k < Count; i++)
        {
            if (i >= _items.Count || _freeSet.Contains(i))
                continue;

            yield return _items[i];
            k++;
        }
    }
}
