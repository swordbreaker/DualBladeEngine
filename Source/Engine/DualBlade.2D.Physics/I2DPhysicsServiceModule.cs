using DualBlade._2D.Physics.Services;
using DualBlade._2D.Physics.Systems;
using Jab;

namespace DualBlade._2D.Physics
{
    [ServiceProviderModule]
    [Singleton<IPhysicsManager, PhysicsManager>]
    [Transient<KinematicSystem>]
    [Transient<PhysicSystem>]
    public interface I2DPhysicsServiceModule
    {
    }
}
