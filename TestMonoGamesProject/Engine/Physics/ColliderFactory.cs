using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace TestMonoGamesProject.Engine.Physics
{
    public class ColliderFactory : IColliderFactory
    {
        public ICollider CreateRectColliderFormTexture(Texture2D texture)
        {
            var pixels = new Color[texture.Width * texture.Height];
            texture.GetData(pixels);

            var (l, t, r, b) = (float.MaxValue, float.MaxValue, float.MinValue, float.MinValue);

            for (var y = 0; y < texture.Height; y++)
            {
                for (var x = 0; x < texture.Width; x++)
                {
                    if (pixels[y * texture.Width + x].A > 0)
                    {
                        l = Math.Min(l, x);
                        t = Math.Min(t, y);
                        r = Math.Max(r, x);
                        b = Math.Max(b, y);
                    }
                }
            }

            return new RectCollider(new RectangleF(l, t, r - l, b - t));
        }
    }
}
