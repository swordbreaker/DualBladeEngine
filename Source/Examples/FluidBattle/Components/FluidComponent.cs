using DualBlade.Core.Components;

namespace FluidBattle.Components;

public partial struct FluidComponent : IComponent
{
    public int Player;
    public Vector2 Target;
    public Color Color;
}