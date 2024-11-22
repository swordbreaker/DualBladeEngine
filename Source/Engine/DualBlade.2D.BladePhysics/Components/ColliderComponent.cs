using DualBlade._2D.BladePhysics.Models;
using DualBlade.Core.Components;

namespace DualBlade._2D.BladePhysics.Components;

public partial struct ColliderComponent : IComponent
{
    public ColliderComponent(params ICollider[] colliders) =>
        Colliders = colliders;

    public ICollider[] Colliders = [];
}
