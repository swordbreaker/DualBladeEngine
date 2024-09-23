using DualBlade.Core.Systems;
using Services;
namespace Systems;

public sealed class PhysicSystem(IPhysicsManager physicsManager) : ISystem
{
    public void Dispose() { }
    public void Draw(GameTime gameTime) { }
    public void Initialize() { }

    public void Update(GameTime gameTime)
    {
        physicsManager.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
    }
}
