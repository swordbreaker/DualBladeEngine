using DualBlade._2D.Physics.Services;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Physics.Systems;

public sealed class PhysicSystem(IGameContext context, IPhysicsManager physicsManager) : BaseSystem(context)
{
    public override void FixedUpdate(GameTime gameTime)
    {
        physicsManager.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}