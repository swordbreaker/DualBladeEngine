namespace DualBlade.Core.Collections;

public class GrowableMemory<T>
{
    private Memory<T> _memory = new T[10];
    private int _length;

    public int Length => _length;

    public GrowableMemory()
    {
        _length = 0;
    }

    public GrowableMemory(int capacity)
    {
        _memory = new T[capacity];
        _length = 0;
    }

    public ref T this[int index] => ref _memory.Span[index];

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

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public void Remove(int index)
    {
        if (index < 0 || index >= Length)
        {
            throw new IndexOutOfRangeException();
        }

        for (int i = index; i < Length - 1; i++)
        {
            _memory.Span[i] = _memory.Span[i + 1];
        }

        _length--;
    }

    public void Clear() => _length = 0;

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

    public bool Contains(T item)
    {
        var span = _memory.Span[.._length];
        for (int i = 0; i < _length; i++)
        {
            if (span[i].Equals(item))
            {
                return true;
            }
        }

        return false;
    }

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

    public Memory<T> ToMemory() => _memory[..Length];
    public Span<T> ToSpan() => _memory.Span[..Length];
}
