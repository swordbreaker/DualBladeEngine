using DualBlade._2D.Rendering.Entities;
using DualBlade.Core.Entities;
using DualBlade.Core.Extensions;
using DualBlade.Core.Scenes;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using ExampleGame.Entities;
using ExampleGame.Systems;
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

    protected override IEnumerable<IEntity> SetupEntities()
    {
        var (w, h) = gameEngine.GameSize;

        var ball = CreateEntity<BallEntity>();
        var ballChild = CreateEntity<SpriteEntity>();
        ballChild.Renderer.SetSprite(gameEngine.CreateSprite("ball"));
        ballChild.Transform.Position = new Vector2(0, 1f);
        ball.AddChild(ballChild);

        yield return ball;

        yield return this.CreateEntity<GroundEntity>();

        var left = this.CreateEntity<GroundEntity>();
        left.Color = Color.Black;
        left.Position = new Vector2(-w / 2, 0);
        left.Size = new Vector2(2, h / left.Renderer.Sprite.Height);

        var right = this.CreateEntity<GroundEntity>();
        right.Color = Color.Black;
        right.Position = new Vector2(w / 2, 0);
        right.Size = new Vector2(2, h / right.Renderer.Sprite.Height);

        yield return left;
        yield return right;
    }
}
