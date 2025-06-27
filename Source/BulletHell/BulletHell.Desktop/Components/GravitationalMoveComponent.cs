using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace BulletHell.Desktop.Components;

public partial struct GravitationalMoveComponent : IComponent
{
    public Vector2 Velocity { get; set; }
    public Vector2 GravityCenter { get; set; }
    public float GravityStrength { get; set; }
    public float Mass { get; set; }
    public bool IsAttraction { get; set; } // true for attraction, false for repulsion
    public float MinDistance { get; set; } // Minimum distance to prevent infinite acceleration
    public float MaxForce { get; set; }
}
