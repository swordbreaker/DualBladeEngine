using BulletHell.Desktop.Components;
using BulletHell.Desktop.Factories;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class ClusterBulletSystem(IGameContext context) : ComponentSystem<TransformComponent, ClusterBulletComponent>(context)
{
    protected override void OnAdded(ref IEntity entity, ref TransformComponent transform, ref ClusterBulletComponent cluster)
    {
        cluster.InitialPosition = transform.Position;
    }

    protected override void Update(ref TransformComponent transform, ref ClusterBulletComponent cluster, ref IEntity entity, GameTime gameTime)
    {
        // Check if bullet has traveled far enough to split
        if (!cluster.HasSplit)
        {
            var distanceTraveled = Vector2.Distance(transform.Position, cluster.InitialPosition);

            if (distanceTraveled >= cluster.SplitDistance)
            {
                SplitBullet(transform.Position, cluster, entity);
                cluster.HasSplit = true;

                // Mark the original bullet for destruction
                Ecs.DestroyEntity(entity);
            }
        }
    }

    private void SplitBullet(Vector2 position, ClusterBulletComponent cluster, IEntity originalEntity)
    {
        var bulletFactory = new BulletFactory(GameContext);

        // Create bullets in a circle pattern
        var angleStep = MathHelper.TwoPi / cluster.ClusterCount;
        var baseSpeed = 100f; // Speed for cluster bullets

        for (int i = 0; i < cluster.ClusterCount; i++)
        {
            var angle = i * angleStep;
            var velocity = new Vector2(
                MathF.Cos(angle) * baseSpeed,
                MathF.Sin(angle) * baseSpeed
            );

            var color = Color.Orange; // Distinct color for cluster bullets
            var bullet = bulletFactory.CreateBullet(BulletType.Linear, position, velocity, color, 6);

            World.AddEntity(bullet);
        }
    }
}
