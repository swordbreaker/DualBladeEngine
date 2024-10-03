using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace FluidBattle.Components;

public partial struct FluidPixelComponent : IComponent
{
    public float MinRadius = 0.05f;
    public float MaxRadius;
    public Vector2 Target;
}
