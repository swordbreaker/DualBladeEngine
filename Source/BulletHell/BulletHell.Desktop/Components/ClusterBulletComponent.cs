using DualBlade.Core.Components;

namespace BulletHell.Desktop.Components;

public partial struct ClusterBulletComponent : IComponent
{
    public float SplitDistance; // Distance traveled before splitting
    public int ClusterCount; // Number of bullets to split into
    public bool HasSplit; // Has this bullet already split?
    public Vector2 InitialPosition; // Starting position to track distance
}
