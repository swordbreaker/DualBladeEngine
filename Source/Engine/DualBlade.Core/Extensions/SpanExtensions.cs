namespace DualBlade.Core.Extensions;

public static class SpanExtensions
{
    /// <summary>
    /// Check if the span contains the value.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
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