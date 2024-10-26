using DualBlade._2D.BladePhysics.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluidBattle;
public class PhysicsSettings : IPhysicsSettings
{
    public IGridSettings GridSettings { get; } = new UniformGirdSettings(CellSize: 9 / 32, Width: );

    public Vector2 Gravity { get; } = Vector2.Zero;
}
