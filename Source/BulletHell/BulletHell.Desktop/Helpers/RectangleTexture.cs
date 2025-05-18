using System;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop.Helpers;


public record RectangleProperties
{
    public float Width { get; init; }
    public float Height { get; init; }

    public Color StrokeColor { get; init; } = Color.White;
    public Color FillColor { get; init; } = Color.Transparent;
    public int StrokeThickness { get; init; } = 1;
}

public class RectangleTexture : Texture2D
{
    public RectangleTexture(GraphicsDevice graphicsDevice, RectangleProperties rectangleProperties)
        : base(graphicsDevice, (int)rectangleProperties.Width, (int)rectangleProperties.Height)
    {
        var width = rectangleProperties.Width;
        var height = rectangleProperties.Height;
        var strokeColor = rectangleProperties.StrokeColor;
        var fillColor = rectangleProperties.FillColor;
        var strokeThickness = rectangleProperties.StrokeThickness;

        // Ensure stroke thickness is valid
        strokeThickness = Math.Min(strokeThickness, (int)Math.Min(width, height));

        var data = new Color[Width * Height];

        // Initialize all pixels as transparent
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = Color.Transparent;
        }

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                // Check if the pixel is part of the border
                if ((x < strokeThickness || x >= Width - strokeThickness || y < strokeThickness || y >= Height - strokeThickness))
                {
                    data[x + y * Width] = strokeColor;
                }
                // Otherwise it's part of the fill
                else
                {
                    data[x + y * Width] = fillColor;
                }
            }
        }

        SetData(data);
    }
}
