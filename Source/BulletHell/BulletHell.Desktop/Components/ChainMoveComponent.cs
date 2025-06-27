using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BulletHell.Desktop.Components;

public partial struct ChainMoveComponent : IComponent
{
    public Vector2 Velocity { get; set; }
    public List<IEntity> ChainLinks { get; set; }
    public bool IsChainHead { get; set; }
    public IEntity ChainParent { get; set; }
    public float ChainLinkDistance { get; set; }
    public float ChainStiffness { get; set; } // How rigid the chain is
    public int ChainIndex { get; set; } // Position in chain (0 = head)
    public Vector2 LastPosition { get; set; }
}
