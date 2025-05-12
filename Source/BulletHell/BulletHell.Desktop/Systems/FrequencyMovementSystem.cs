using System;
using BulletHell.Desktop.Components;
using DualBlade._2D.Rendering.Components;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;

namespace BulletHell.Desktop.Systems;

public class FrequencyMovementSystem(IGameContext context) : ComponentSystem<TransformComponent, FrequencyMovementComponent>(context)
{
    protected override void Update(ref TransformComponent transform, ref FrequencyMovementComponent frequencyMovement, ref IEntity entity, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Calculate the new position based on frequency and decibel
        var frequency = frequencyMovement.Frequency;
        var decibel = frequencyMovement.Decibel;

        // Example calculation: adjust position based on frequency and decibel
        transform.Position.X += (float)(Math.Sin(frequency * gameTime.TotalGameTime.TotalSeconds) * decibel * deltaTime);
    }
}
