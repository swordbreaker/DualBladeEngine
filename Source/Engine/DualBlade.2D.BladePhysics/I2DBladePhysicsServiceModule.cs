using DualBlade._2D.BladePhysics.Services;
using DualBlade._2D.BladePhysics.Systems;
using Jab;

namespace DualBlade._2D.BladePhysics;

[ServiceProviderModule]
[Singleton<IPhysicsSettings, PhysicsSettings>]
[Singleton<IPhysicsManager, PhysicsManager>]
[Transient<RigidBodySystem>]
[Transient<ColliderSystem>]
public interface I2DBladePhysicsServiceModule
{
}
