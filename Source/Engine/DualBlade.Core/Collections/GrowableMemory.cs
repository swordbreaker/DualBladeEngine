namespace DualBlade.Core.Collections;

/// <summary>
/// Represents a growable collection that provides direct memory access with automatic resizing.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public class GrowableMemory<T>
{
    private Memory<T> _memory = new T[10];
    private int _length;

    /// <summary>
    /// Gets the number of elements contained in the collection.
    /// </summary>
    public int Length => _length;

    /// <summary>
    /// Initializes a new instance of the <see cref="GrowableMemory{T}"/> class with default capacity.
    /// </summary>
    public GrowableMemory()
    {
        _length = 0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GrowableMemory{T}"/> class with the specified capacity.
    /// </summary>
    /// <param name="capacity">The initial capacity of the collection.</param>
    public GrowableMemory(int capacity)
    {
        _memory = new T[capacity];
        _length = 0;
    }

    /// <summary>
    /// Gets or sets the element at the specified index as a reference.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get or set.</param>
    /// <returns>A reference to the element at the specified index.</returns>
    public ref T this[int index] => ref _memory.Span[index];

    /// <summary>
    /// Adds an item to the end of the collection.
    /// </summary>
    /// <param name="item">The item to add.</param>
    /// <returns>The index at which the item was added.</returns>
    public int Add(T item)
    {
        if (Length == _memory.Length)
        {
            var newMemory = new T[_memory.Length * 2];
            _memory.Span.CopyTo(newMemory);
            _memory = newMemory;
        }

        _memory.Span[Length] = item;
        return _length++;
    }

    /// <summary>
    /// Adds a collection of items to the end of the collection.
    /// </summary>
    /// <param name="items">The items to add.</param>
    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    /// <summary>
    /// Removes the element at the specified index and shifts all subsequent elements down.
    /// </summary>
    /// <param name="index">The zero-based index of the element to remove.</param>
    /// <exception cref="IndexOutOfRangeException">Thrown when the index is outside the bounds of the collection.</exception>
    public void Remove(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        for (int i = index; i < Length - 1; i++)
        {
            _memory.Span[i] = _memory.Span[i + 1];
        }

        _length--;
    }

    /// <summary>
    /// Clears all elements from the collection.
    /// </summary>
    public void Clear() => _length = 0;

    /// <summary>
    /// Determines whether the collection contains an element that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match against elements.</param>
    /// <returns>true if an element matching the predicate is found; otherwise, false.</returns>
    public bool Contains(Predicate<T> predicate)
    {
        var span = _memory.Span[.._length];
        for (int i = 0; i < _length; i++)
        {
            if (predicate(span[i]))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether the collection contains the specified item.
    /// </summary>
    /// <param name="item">The item to locate.</param>
    /// <returns>true if the item is found; otherwise, false.</returns>
    public bool Contains(T item)
    {
        var span = _memory.Span[.._length];
        for (int i = 0; i < _length; i++)
        {
            if (EqualityComparer<T>.Default.Equals(span[i], item))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Searches for an element that matches the specified predicate and returns the first occurrence.
    /// </summary>
    /// <param name="predicate">The predicate to match against elements.</param>
    /// <returns>The first element that matches the predicate.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no element matches the predicate.</exception>
    public T Find(Predicate<T> predicate)
    {
        var span = _memory.Span[.._length];
        for (int i = 0; i < _length; i++)
        {
            if (predicate(span[i]))
            {
                return span[i];
            }
        }

        throw new InvalidOperationException("Item not found");
    }

    /// <summary>
    /// Tries to find an element that matches the specified predicate.
    /// </summary>
    /// <param name="predicate">The predicate to match against elements.</param>
    /// <param name="item">When this method returns, contains the found element if successful, or default value if not.</param>
    /// <returns>true if an element matching the predicate is found; otherwise, false.</returns>
    public bool TryFind(Predicate<T> predicate, out T item)
    {
        var span = _memory.Span[.._length];
        for (int i = 0; i < _length; i++)
        {
            if (predicate(span[i]))
            {
                item = span[i];
                return true;
            }
        }

        item = default!;
        return false;
    }

    /// <summary>
    /// Returns a <see cref="Memory{T}"/> that represents the utilized portion of the internal buffer.
    /// </summary>
    /// <returns>A memory object representing the utilized portion of the internal buffer.</returns>
    public Memory<T> ToMemory() => _memory[..Length];

    /// <summary>
    /// Returns a <see cref="Span{T}"/> that represents the utilized portion of the internal buffer.
    /// </summary>
    /// <returns>A span representing the utilized portion of the internal buffer.</returns>
    public Span<T> ToSpan() => _memory.Span[..Length];
}
