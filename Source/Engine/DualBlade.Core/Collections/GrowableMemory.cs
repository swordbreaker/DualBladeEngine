namespace DualBlade.Core.Collections;

public struct GrowableMemory<T>
{
    private Memory<T> _memory;

    public int Length { get; private set; }

    public GrowableMemory(int capacity)
    {
        _memory = new T[capacity];
        Length = 0;
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
        return Length++;
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

        Length--;
    }

    public void Clear() => Length = 0;

    public readonly bool Contains(Predicate<T> predicate)
    {
        var span = _memory.Span[..Length];
        for (int i = 0; i < Length; i++)
        {
            if (predicate(span[i]))
            {
                return true;
            }
        }

        return false;
    }

    public readonly bool Contains(T item)
    {
        var span = _memory.Span[..Length];
        for (int i = 0; i < Length; i++)
        {
            if (span[i].Equals(item))
            {
                return true;
            }
        }

        return false;
    }

    public readonly T Find(Predicate<T> predicate)
    {
        var span = _memory.Span[..Length];
        for (int i = 0; i < Length; i++)
        {
            if (predicate(span[i]))
            {
                return span[i];
            }
        }

        throw new InvalidOperationException("Item not found");
    }

    public Memory<T> ToMemory() => _memory[..Length];
    public Span<T> ToSpan() => _memory.Span[..Length];
}
