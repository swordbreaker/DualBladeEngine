using DualBlade.Core.Components;
using Microsoft.Xna.Framework;

namespace BulletHell.Desktop.Components;

public partial struct SeekingMoveComponent : IComponent
{
    public Vector2 Velocity { get; set; }
    public Vector2 TargetPosition { get; set; }
    public float MaxSpeed { get; set; }
    public float SeekingForce { get; set; }
    public float ArrivalRadius { get; set; }
    public float PredictionTime { get; set; } // How far ahead to predict target movement
    public bool HasTarget { get; set; }
    public float LastTargetUpdateTime { get; set; }
    public Vector2 LastTargetPosition { get; set; }
    public Vector2 PredictedTargetVelocity { get; set; }
}
