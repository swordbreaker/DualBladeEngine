using ExampleGame.Components;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Components;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Worlds;
using nkast.Aether.Physics2D.Dynamics;

namespace ExampleGame.Entities;

public class BallEntity : SpriteEntity
{
    public static float G = 9.81f * 50;

    public KinematicComponent KinematicComponent { get; }
    public CharacterComponent CharacterComponent { get; }

    public BallEntity(IGameEngine gameEngine)
    {
        KinematicComponent = AddComponent<KinematicComponent>();
        CharacterComponent = AddComponent<CharacterComponent>();
        
        Transform!.Position = gameEngine.GameSize/2;
        Renderer.SetTexture(gameEngine.Load<Texture2D>("ball"));

        // Create a physics body
        var body = gameEngine.PhysicsManager.CreateBody(Transform.Position, bodyType: BodyType.Dynamic);
        body.Tag = this;
        body.FixedRotation = true;
        body.Mass = 1;
        body.LinearDamping = 0f;
        var fixture = body.CreateCircle(Renderer.Texture!.Width/2f, 1);
        fixture.Tag = this;
        //fixture.Restitution = 0.5f;
        //fixture.Friction = 1f;

        KinematicComponent.PhysicsBody = body;
    }
}
