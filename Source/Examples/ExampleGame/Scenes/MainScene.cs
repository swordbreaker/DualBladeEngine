using DualBlade._2D.Physics.Services;
using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using ExampleGame.Entities;
using ExampleGame.Systems;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xna.Framework;

namespace ExampleGame.Scenes;
public class MainScene(IGameContext context) : GameScene(context)
{
    private readonly IGameEngine gameEngine = context.GameEngine;

    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return CreateSystem<BallSystem>();
        yield return CreateSystem<SpawnSystem>();
    }

    protected override IEnumerable<INodeEntity> SetupEntities()
    {
        var (w, h) = gameEngine.GameSize;
        var physicsManager = GameContext.ServiceProvider.GetRequiredService<IPhysicsManager>();

        var ball = new BallEntity(GameContext, physicsManager);
        var ballChild = new SpriteEntity();
        ballChild.Renderer.SetSprite(gameEngine.CreateSprite("ball"));
        ballChild.Transform.Position = new Vector2(0, 1f);
        ball.AddChild(ballChild);

        yield return ball;

        yield return new GroundEntity(GameContext, physicsManager);

        var left = new GroundEntity(GameContext, physicsManager)
        {
            Color = Color.Black,
            Position = new Vector2(-w / 2, 0)
        };
        left.Size = new Vector2(2, h / left.Renderer.Sprite.Height);

        var right = new GroundEntity(GameContext, physicsManager)
        {
            Color = Color.Black,
            Position = new Vector2(w / 2, 0)
        };
        right.Size = new Vector2(2, h / right.Renderer.Sprite.Height);

        yield return left;
        yield return right;
    }
}
