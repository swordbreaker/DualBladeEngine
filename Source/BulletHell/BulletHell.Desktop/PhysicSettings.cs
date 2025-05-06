using DualBlade._2D.BladePhysics.Services;
using Microsoft.Xna.Framework;

namespace BulletHell.Desktop;

public class PhysicSettings : IPhysicsSettings
{
    public IGridSettings GridSettings => new UniformGirdSettings(CellSize: 9f / 32, Width: 10, Height: 10);

    public Vector2 Gravity => Vector2.Zero;
}
