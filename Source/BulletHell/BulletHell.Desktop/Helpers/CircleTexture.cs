using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop.Helpers;

public record CircleProperties
{
    public required int Radius { get; init; }
    public Color StrokeColor { get; init; } = Color.White;
    public Color FillColor { get; init; } = Color.Transparent;
    public int StrokeThickness { get; init; } = 1;
}

public class CircleTexture : Texture2D
{
    public CircleTexture(GraphicsDevice graphicsDevice, CircleProperties circleProperties)
        : base(graphicsDevice, circleProperties.Radius * 2, circleProperties.Radius * 2)
    {
        var radius = circleProperties.Radius;
        var strokeColor = circleProperties.StrokeColor;
        var fillColor = circleProperties.FillColor;
        var strokeThickness = circleProperties.StrokeThickness;
        
        // Ensure stroke thickness is valid
        strokeThickness = System.Math.Min(strokeThickness, radius);
        
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
                int dx = x - radius;
                int dy = y - radius;
                int distanceSquared = dx * dx + dy * dy;
                
                // Check if the pixel is within the circle
                if (distanceSquared <= radius * radius)
                {
                    // Check if the pixel is part of the border
                    if (distanceSquared >= (radius - strokeThickness) * (radius - strokeThickness))
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
        }
        
        SetData(data);
    }
}