using DualBlade.Core.Components;
using DualBlade.Core.Entities;
using Microsoft.Xna.Framework;

namespace DualBlade.Editor.Player.Components;
public class SelectableComponent : IComponent
{
    public bool IsSelected { get; set; }
    public Rectangle Rect { get; set; }
    public IEntity Entity { get; init; }
}
