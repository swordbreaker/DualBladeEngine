using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class MoveSystem(IGameContext context) : ComponentSystem<TransformComponent, MoveComponent>(context)
{
    protected override void Update(ref TransformComponent component1, ref MoveComponent component2, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var velocity = component2.Velocity * deltaTime;

        component1.Position += velocity;
    }
}

