using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Entities;

namespace MonoGameEngine.Engine.Components;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public record RenderComponent : IComponent
{
    public Color Color = Color.White;
    public Texture2D? Texture;
    public Vector2 Origin = Vector2.Zero;
    public IEntity Entity { get; init; }

    public void SetTexture(Texture2D texture, bool updateOrigin = true)
    {
        Texture = texture;
        if (updateOrigin)
        {
            Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
        }
    }
}
