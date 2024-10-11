using DualBlade._2D.Physics.Services;
using DualBlade.Core.Systems;

namespace DualBlade._2D.Physics.Systems;

public sealed class PhysicSystem(IPhysicsManager physicsManager) : ISystem
{
    public void Dispose() { }
    public void Draw(GameTime gameTime) { }
    public void Initialize() { }

    private float timeSinceLastUpdate = 0;

    public void Update(GameTime gameTime)
    {
        physicsManager.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}
