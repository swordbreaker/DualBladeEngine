
using System.Drawing;

namespace DualBlade.Core.Services;

public interface IWorldToPixelConverter
{
    int TileSize { get; init; }
    Matrix WorldMatrix { get; }
    Matrix WorldSizeMatrix { get; }
    RectangleF WorldBounds { get; }

    Vector2 PixelPointToWorld(Vector2 pixelCoordinate);
    Vector2 PixelSizeToWorld(Vector2 pixelSize);
    Vector2 WorldPointToPixel(Vector2 worldCoordinate);
    Vector2 WorldSizeToPixel(Vector2 worldSize);
}