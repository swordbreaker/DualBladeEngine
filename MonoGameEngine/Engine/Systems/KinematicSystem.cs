using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Worlds;

namespace MonoGameEngine.Engine.Systems;

public class KinematicSystem : ComponentSystem<KinematicComponent>
{
    protected override void Update(KinematicComponent component, GameTime gameTime, IGameEngine gameEngine)
    {
        var entity = component.Entity;
        var transform = World.GetComponent<TransformComponent>(entity)!;

        transform.Position = component.PhysicsBody.Position;
    }
}
