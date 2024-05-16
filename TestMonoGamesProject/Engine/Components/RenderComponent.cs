using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TestMonoGamesProject.Engine.Entities;

namespace TestMonoGamesProject.Engine.Components
{
    public record RenderComponent : IComponent
    {
        public Color Color = Color.White;
        public Texture2D Texture;
        public Vector2 Origin = Vector2.Zero;
        public IEntity Entity { get; init; }

        public void SetTexture(Texture2D texture, bool updateOrigin = true)
        {
            this.Texture = texture;
            if(updateOrigin)
            {
                Origin = new Vector2(texture.Width / 2f, texture.Height / 2f);
            }
        }
    }
}
