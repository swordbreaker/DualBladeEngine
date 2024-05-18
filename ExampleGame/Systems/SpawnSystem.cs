using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Systems;
using MonoGameEngine.Engine.Worlds;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Systems;
internal class SpawnSystem : BaseSystem
{
    public override void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        if (gameEngine.InputManager.IsKeyJustPressed(Keys.Z))
        {
            CrateBall(gameEngine);
        }
    }

    private void CrateBall(IGameEngine gameEngine)
    {
        var (w, h) = gameEngine.GameSize;

        var entity = new TransformEntity() { World = World };
        var kinematic = entity.AddComponent<KinematicComponent>();
        var renderer = entity.AddComponent<RenderComponent>();

        var x = Random.Shared.Next(0, (int)w);
        var y = 0;
        var pos = new Vector2(x, y);

        renderer.SetTexture(gameEngine.Load<Texture2D>("ball"));
        kinematic.PhysicsBody = gameEngine.PhysicsManager.CreateBody(pos, bodyType: BodyType.Dynamic);
        kinematic.PhysicsBody.CreateCircle(renderer.Texture!.Width / 2f, 1);

        entity.Transform.Position = pos;

        World.AddEntity(entity);
    }
}
