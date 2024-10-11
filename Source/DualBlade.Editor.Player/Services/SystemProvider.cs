using DualBlade._2D.Physics.Services;
using DualBlade._2D.Physics.Systems;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using System;
using System.Collections.Generic;

namespace DualBlade.Editor.Player.Services;
public class SystemProvider(IGameContext gameContext, IPhysicsManager physicsManager)
{
    private readonly Lazy<List<ISystem>> _systems = new(() => [
            new PhysicSystem(physicsManager),
            new KinematicSystem(gameContext),
        ]);

    public List<ISystem> Systems => _systems.Value;
}
