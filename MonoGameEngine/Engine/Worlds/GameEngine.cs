﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Services;

namespace MonoGameEngine.Engine.Worlds;

public record GameEngine : IGameEngine
{
    public required IPhysicsManager PhysicsManager { get; init; }
    public required GraphicsDeviceManager GraphicsDeviceManager { get; init; }
    public required InputManager InputManager { get; init; }
    public required ContentManager Content { get; init; }
    public SpriteBatch SpriteBatch { get; private set; }

    public CameraService CameraService { get; init; }

    public Vector2 GameSize =>
        new(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight);

    public void Initialize()
    {
        SpriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
    }

    public void BeginDraw() =>
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: CameraService.TransformMatrix);

    public void Draw(
        Texture2D texture,
        Vector2 position,
        Color? color = null,
        Rectangle? sourceRectangle = null,
        float rotation = 0f,
        Vector2? origin = null,
        Vector2? scale = null,
        SpriteEffects effects = SpriteEffects.None,
        float layerDepth = 0f)
    {
        color ??= Color.White;
        origin ??= new Vector2(texture.Width / 2, texture.Height / 2);
        scale ??= Vector2.One;

        SpriteBatch.Draw(
                texture,
                position,
                sourceRectangle: sourceRectangle,
                color: color.Value,
                rotation: rotation,
                origin: origin.Value,
                scale: scale.Value,
                effects: effects,
                layerDepth: layerDepth);
    }

    public void EndDraw() => SpriteBatch.End();

    public T Load<T>(string assetName) =>
        Content.Load<T>(assetName);

}
