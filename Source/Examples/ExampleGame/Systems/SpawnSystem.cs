using DualBlade._2D.Physics.Components;
using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Components;
using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Systems;
internal class SpawnSystem(IGameContext gameContext, IPhysicsManager physicsManager) : BaseSystem(gameContext)
{
    private readonly IGameEngine gameEngine = gameContext.GameEngine;

    public override void Update(GameTime gameTime)
    {
        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Z))
        {
            CrateBall();
        }
    }

    private void CrateBall()
    {
        var (w, _) = gameEngine.GameSize;

        var entity = new TransformEntity();
        var kinematic = entity.AddComponent<KinematicComponent>();
        var renderer = entity.AddComponent<RenderComponent>();

        var x = Random.Shared.Next(0, (int)w);
        var y = 0;
        var pos = new Vector2(x, y);

        renderer.SetSprite(gameEngine.CreateSprite("ball"));
        kinematic.PhysicsBody = physicsManager.CreateBody(pos, bodyType: BodyType.Dynamic);
        kinematic.PhysicsBody.CreateCircle(renderer.Sprite!.Width / 2f, 1);

        entity.Transform.Position = pos;

        World.AddEntity(entity);
    }
}
