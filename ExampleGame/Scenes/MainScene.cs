using ExampleGame.Entities;
using ExampleGame.Systems;
using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Extensions;
using MonoGameEngine.Engine.Scenes;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;

namespace ExampleGame.Scenes;
public class MainScene(IWorld world, IGameEngine gameEngine) : GameScene(world)
{
    public override IEnumerable<ISystem> SetupSystems()
    {
        yield return new BallSystem(gameEngine.CameraService) { World = World };
        yield return CreateSystem<SpawnSystem>();
    }

    protected override IEnumerable<IEntity> SetupEntities()
    {
        var (w, h) = gameEngine.GameSize;

        var ball = new BallEntity(gameEngine)
        {
            World = World,
        };
        var ballChild = new SpriteEntity() { World = World };
        ballChild.Renderer.SetSprite(gameEngine.CreateSprite("ball"));
        ballChild.Transform.Position = new Vector2(0, 1f);
        ball.AddChild(ballChild);

        yield return ball;

        yield return new GroundEntity(gameEngine)
        {
            World = World,
        };

        var left = new GroundEntity(gameEngine)
        {
            World = World,
            Color = Color.Black,
            Position = new Vector2(-w / 2, 0),
        };
        left.Size = new Vector2(2, h / left.Renderer.Sprite.Height);

        var right = new GroundEntity(gameEngine)
        {
            World = World,
            Color = Color.Black,
            Position = new Vector2(w / 2, 0),
        };
        right.Size = new Vector2(2, h / right.Renderer.Sprite.Height);

        yield return left;
        yield return right;
    }
}
