
namespace MonoGameEngine.Engine.Services;

public interface IWorldToPixelConverter
{
    Vector2 PixelPointToWorld(Vector2 pixelCoordinate);
    Vector2 PixelSizeToWorld(Vector2 pixelSize);
    Vector2 WorldPointToPixel(Vector2 worldCoordinate);
    Vector2 WorldSizeToPixel(Vector2 worldSize);
}