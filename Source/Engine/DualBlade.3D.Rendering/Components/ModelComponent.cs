using DualBlade.Core.Components;
using Microsoft.Xna.Framework.Graphics;

namespace DualBlade._3D.Rendering.Components;

public partial struct ModelComponent : IComponent
{
    public Model Model { get; set; }
}