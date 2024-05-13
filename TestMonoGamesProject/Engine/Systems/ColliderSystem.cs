using Microsoft.Xna.Framework;
using System.Linq;
using TestMonoGamesProject.Components;
using TestMonoGamesProject.Engine.Worlds;

namespace TestMonoGamesProject.Engine.Systems;

public class ColliderSystem : BaseSystem
{
    public override void Update(GameTime gameTime, IGameEngine gameEngine)
    {
        gameEngine.PhysicsManager.Clear();
        gameEngine.PhysicsManager.AddColliders(World.GetComponents<ColliderComponent>().Select(x => x.Collider));
    }
}
