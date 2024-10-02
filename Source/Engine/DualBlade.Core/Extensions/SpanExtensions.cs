namespace DualBlade.Core.Extensions;
public static class SpanExtensions
{
    public static bool Contains<T>(this Span<T> span, T value)
    {
        foreach (var item in span)
        {
            if (item.Equals(value))
            {
                return true;
            }
        }
        return false;
    }
}
