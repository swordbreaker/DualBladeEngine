using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class ZigzagMoveSystem(IGameContext gameContext) : ComponentSystem<ZigzagMoveComponent, TransformComponent>(gameContext)
{
    protected override void Update(ref ZigzagMoveComponent zigzag, ref TransformComponent transform, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        zigzag.CurrentTime += deltaTime;

        // Calculate zigzag offset
        var zigzagOffset = (float)Math.Sin(zigzag.CurrentTime * zigzag.ZigzagFrequency) * zigzag.ZigzagAmplitude;

        // Calculate movement vector
        var movement = zigzag.BaseDirection * zigzag.BaseSpeed * deltaTime;
        var zigzagMovement = zigzag.PerpendicularDirection * zigzagOffset * deltaTime;

        transform.Position += movement + zigzagMovement;
    }
}
