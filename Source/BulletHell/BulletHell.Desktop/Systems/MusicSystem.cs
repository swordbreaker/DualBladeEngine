using System;
using System.Diagnostics;
using System.IO;
using BulletHell.Desktop.Components;
using BulletHell.Desktop.Entities;
using BulletHell.Desktop.Helpers;
using DualBlade.Core.Entities;
using DualBlade.Core.Services;
using DualBlade.Core.Systems;
using FmodForFoxes;
using Microsoft.Xna.Framework.Graphics;

namespace BulletHell.Desktop.Services;


public class MusicSystem(IGameContext context) : ComponentSystem<MusicComponent>(context)
{
    private readonly FeatureExtraction featureExtraction = new FeatureExtraction();
    private readonly IGameEngine gameEngine = context.GameEngine;

    private readonly MusicFeatureConverter musicFeatureConverter = new(context.GameEngine.GameSize);

    private Texture2D _pixel;
    private float lastFeatureValue = 0f;

    public override void Initialize()
    {
        var nativeLibrary = new DesktopNativeFmodLibrary();
        FmodManager.Init(nativeLibrary, FmodInitMode.CoreAndStudio, "./");


        _pixel = new(gameEngine.SpriteBatch.GraphicsDevice, 1, 1);
        _pixel.SetData([Color.White]);
    }

    protected override void OnAdded(ref IEntity entity, ref MusicComponent component)
    {
        var features = featureExtraction.Extract(component.Signal);
        var points = featureExtraction.GenerateSpecPoint(component.Signal, 10);
        
        component.Features = features;
        component.SpectrogramPoints = points;

        var absolutePath = Path.GetFullPath(component.FilePath);

        var result = CoreSystem.Native.createSound(
            absolutePath,
            FMOD.MODE.CREATESTREAM | FMOD.MODE.ACCURATETIME | FMOD.MODE.LOOP_NORMAL,
            out var sound
        );

        if (result != FMOD.RESULT.OK)
        {
            throw new Exception($"FMOD error: {result}");
        }

        CoreSystem.Native.getMasterChannelGroup(out var masterGroup);
        CoreSystem.Native.playSound(sound, masterGroup, false, out var channel);

        sound.getLength(out var length, FMOD.TIMEUNIT.MS);

        component.Channel = channel;
        component.Length = length;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        FmodManager.Update();
    }

    public override void Dispose()
    {
        base.Dispose();
        FmodManager.Unload();
    }

    private void SpanwBullet(float x)
    {
        var cirlceProps = new CircleProperties
        {
            Radius = 20,
            StrokeColor = Color.Black,
            FillColor = Color.Red,
        };

        var pos = new Vector2(x, gameEngine.GameSize.Y / 2);
        var velocity = new Vector2(0, -1);

        World.AddEntity(new BulletEntity(pos, velocity, cirlceProps, GameContext));
    }

    protected override void Update(ref MusicComponent component, ref IEntity entity, GameTime gameTime)
    {
        base.Update(ref component, ref entity, gameTime);
        musicFeatureConverter.Update(component);

        foreach (var point in musicFeatureConverter.GetCurrentSpectrogramBullets())
        {
            SpanwBullet(point.X);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        var centroidColor = musicFeatureConverter.CurrentCentriodColor;
        var spreadColor = musicFeatureConverter.CurrentSpreadColor;
        var decreaseColor = musicFeatureConverter.CurrentDecreaseColor;
        var rmsColor = musicFeatureConverter.CurrentRMSColor;
        var zcrColor = musicFeatureConverter.CurrentZCRolor;

        var spriteFont = gameEngine.Load<SpriteFont>("DefaultFont");

        gameEngine.BeginDraw();

        gameEngine.DrawString(spriteFont, "Centroid", new Vector2(-5, 3.2f), Color.Black);
        gameEngine.DrawString(spriteFont, "Spread", new Vector2(0, 3.2f), Color.Black);
        gameEngine.DrawString(spriteFont, "Decrease", new Vector2(5, 3.2f), Color.Black);
        gameEngine.DrawString(spriteFont, "RMS", new Vector2(-5, -3.4f), Color.Black);
        gameEngine.DrawString(spriteFont, "ZCR", new Vector2(0, -3.4f), Color.Black);
        
        gameEngine.Draw(_pixel, new Vector2(-4, 2), centroidColor, sourceRectangle: new Rectangle(0, 0, 2, 2));
        gameEngine.Draw(_pixel, new Vector2(0, 2), spreadColor, sourceRectangle: new Rectangle(0, 0, 2, 2));
        gameEngine.Draw(_pixel, new Vector2(4, 2), decreaseColor, sourceRectangle: new Rectangle(0, 0, 2, 2));
        gameEngine.Draw(_pixel, new Vector2(-4, -2), rmsColor, sourceRectangle: new Rectangle(0, 0, 2, 2));
        gameEngine.Draw(_pixel, new Vector2(0, -2), zcrColor, sourceRectangle: new Rectangle(0, 0, 2, 2));

        gameEngine.EndDraw();
    }
}