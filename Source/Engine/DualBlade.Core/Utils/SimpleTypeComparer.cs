namespace DualBlade.Core.Utils;
public struct SimpleTypeComparer : IComparer<Type>
{
    public readonly int Compare(Type x, Type y)
    {
        if (x == y)
        {
            return 0;
        }

        if (x.FullName is null)
        {
            return -1;
        }

        if (y.FullName is null)
        {
            return 1;
        }

        return x.FullName.CompareTo(y.FullName);
    }
}
