using DualBlade._2D.Physics.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Physics.Systems;

public sealed class PhysicSystem(IPhysicsManager physicsManager) : FixedSystem
{
    public override void Update(GameTime gameTime)
    {
        physicsManager.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}
