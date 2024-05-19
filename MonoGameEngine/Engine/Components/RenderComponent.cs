using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Entities;
using MonoGameEngine.Engine.Rendering;
using nkast.Aether.Physics2D.Dynamics;

namespace MonoGameEngine.Engine.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record RenderComponent : IComponent
{
    public Color Color = Color.White;
    public ISprite? Sprite;
    public Vector2 Origin = Vector2.Zero;
    public IEntity Entity { get; init; }

    public void SetSprite(ISprite sprite, bool updateOrigin = true)
    {
        Sprite = sprite;
        if (updateOrigin)
        {
            Origin = new Vector2(sprite.Width / 2f, sprite.Height / 2f);
        }
    }
}
