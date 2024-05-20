﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGameEngine.Engine.Physics;
using MonoGameEngine.Engine.Rendering;
using MonoGameEngine.Engine.Services;
using System;

namespace MonoGameEngine.Engine.Worlds;

public record GameEngine(ISpriteFactory _spriteFactory) : IGameEngine
{
    public required IPhysicsManager PhysicsManager { get; init; }
    public required IWorldToPixelConverter WorldToPixelConverter { get; init; }
    public required GraphicsDeviceManager GraphicsDeviceManager { get; init; }
    public required InputManager InputManager { get; init; }
    public required ContentManager Content { get; init; }
    public required ICameraService CameraService { get; init; }
    public SpriteBatch? SpriteBatch { get; private set; }

    public Vector2 GameSize =>
        WorldToPixelConverter.PixelSizeToWorld(
            new(GraphicsDeviceManager.PreferredBackBufferWidth, GraphicsDeviceManager.PreferredBackBufferHeight));

    public void Initialize()
    {
        SpriteBatch = new SpriteBatch(GraphicsDeviceManager.GraphicsDevice);
    }

    public void BeginDraw()
    {
        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        //var transform =  WorldToPixelConverter.WorldMatrix * CameraService.TransformMatrix;
        //var transform = Matrix.Invert(WorldToPixelConverter.WorldMatrix);

        var transform = CameraService.PixelTransformMatrix;
        //transform = Matrix.Identity;
        SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, transformMatrix: transform);
    }

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

        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        position = WorldToPixelConverter.WorldPointToPixel(position);
        origin = WorldToPixelConverter.WorldSizeToPixel(origin.Value);

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

    public void EndDraw()
    {
        if (SpriteBatch is null)
            throw new InvalidOperationException("SpriteBatch is not initialized");

        SpriteBatch.End();
    }

    public T Load<T>(string assetName) =>
        Content.Load<T>(assetName);
    public ISprite CreateSprite(string assetName) =>
        _spriteFactory.CreateSprite(Load<Texture2D>(assetName));

    public ISprite CreateSprite(Texture2D texture2D) => 
        _spriteFactory.CreateSprite(texture2D);
}
